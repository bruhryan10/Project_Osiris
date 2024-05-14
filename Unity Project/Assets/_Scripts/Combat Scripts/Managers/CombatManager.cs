using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CombatManager : MonoBehaviour
{
    PlayerInput input;
    [Header("Script References")]
    [SerializeField] CombatInputManager combatInputManager;
    [SerializeField] EnemyStats enemy;
    [SerializeField] TurnManager turnManager;
    [SerializeField] PlayerMovement playerScript;
    [SerializeField] TileMovement tiles;
    [SerializeField] CombatActions combatA;
    [SerializeField] CombatRooms rooms;
    [SerializeField] EnemyManager enemyM;
    [SerializeField] NewCombatUI combatUI;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] PlayerStats tankStats;
    [SerializeField] PlayerStats supportStats;
    [SerializeField] PartyManager party;
    [SerializeField] PlayerAbility playerAb;
    [SerializeField] TankAbility tankAb;
    [SerializeField] InputDetection inputDetect;
    [SerializeField] AbilityManager abilityM;
    [SerializeField] ObjectiveManager objectiveM;
    [SerializeField] CombatCamera combatCamera;

    [SerializeField] bool isInCombat = false;
    bool enemySpawn;

    //Action Bools
    [SerializeField] bool canMove;
    bool canAction;
    [SerializeField] bool canAbility;
    [SerializeField] bool canAttack;
    bool getRange;

    Vector3 worldPos;
    Quaternion worldPosRot;

    //Cancel Movement objects
    Vector3 currentPos;
    [SerializeField] Vector3[] playerSpawns;
    [SerializeField] Vector3[] supportSpawns;
    [SerializeField] Vector3[] tankSpawns;

    [SerializeField] Vector2[] playerXYSpawns;
    [SerializeField] Vector2[] supportXYSpawns;
    [SerializeField] Vector2[] tankXYSpawns;
    [SerializeField] Quaternion[] rotationSpawns;

    int currentPlayerX;
    int currentPlayerY;
    int tempCur;

    [SerializeField] int abilityCount;
    [SerializeField] int turnCount;
    [SerializeField] GameObject normalCam;
    [SerializeField] GameObject combatCam;
    [SerializeField] GameObject playerTile;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject player;

    [SerializeField] GameObject currentEnemy;
    [SerializeField] GameObject enemyManager;


    [SerializeField] float timer;
    [SerializeField] GameObject startCombat;
    [SerializeField] string objective;


    bool delay;
    Dictionary<int, string> roomDictonary = new Dictionary<int, string>
    {
        { 1, "LEADER" },
        { 2, "KILL_ALL" },
        { 3, "POI" },
        { 4, "FINAL_BOSS" },
    };
    void Start()
    {
        playerStats.SetStat("currentHealth", playerStats.GetStat("health"));
        tankStats.SetStat("currentHealth", tankStats.GetStat("health"));
        supportStats.SetStat("currentHealth", supportStats.GetStat("health"));
        input = player.GetComponent<PlayerInput>();
        combatCam.SetActive(false);
        playerTile.SetActive(false);
    }
    public void StartCombat(string enemy)
    {
        objective = enemy;
        if (objective != "FINAL_BOSS")
            objectiveM.RandomSubObjective();
        combatUI.DefaultState();
        tempCur = playerStats.GetCurrency();
        currentEnemy.SetActive(false);
        GetWorldPos();
        rooms.SetRoom(objective);
        SetSpawns(objective);
        SetCombatState(true);
        SetEnemyState(true);
        SetActionState(true);
        playerScript.move = false;
        playerTile.SetActive(true);
        normalCam.SetActive(false);
        combatCam.SetActive(true);
        input.actions.FindActionMap("General").Disable();
        input.actions.FindActionMap("Combat").Enable();
        combatUI.TurnStatus();
        enemyM.SpawnEnemy(objective);
        Debug.Log("COMBAT START");
    }
    public void RestartCombat()
    {
        EndTurn();
        abilityM.ResetAbilities();
        turnManager.clearTurnOrder();
        enemyManager.SetActive(true);
        SetWorldPos();
        playerScript.move = true;
        SetCombatState(false);
        SetEnemyState(false);
        SetMoveState(false);

        playerTile.SetActive(false);
        normalCam.SetActive(true);
        combatCam.SetActive(false);
        enemyM.SetHasSpawned(false);
        delay = false;
        input.actions.FindActionMap("General").Enable();
        input.actions.FindActionMap("Combat").Disable();
        playerStats.SetStat("currentHealth", playerStats.GetStat("health"));
        tankStats.SetStat("currentHealth", tankStats.GetStat("health"));
        supportStats.SetStat("currentHealth", supportStats.GetStat("health"));
        combatUI.SetFinished(false);
        tempCur = 0;
        inputDetect.t = false;
        currentEnemy.SetActive(true);
        abilityCount = 0;
        turnCount = 0;
    }
    public void FinishRoom()
    {
        EndTurn();
        AfterCombat();
    }
    public void AfterCombat()
    {
        abilityM.ResetAbilities();
        turnManager.clearTurnOrder();
        enemyManager.SetActive(true);
        Destroy(currentEnemy);
        SetWorldPos();
        playerScript.move = true;
        SetCombatState(false);
        SetEnemyState(false);
        SetMoveState(false);

        playerTile.SetActive(false);
        normalCam.SetActive(true);
        combatCam.SetActive(false);
        enemyM.SetHasSpawned(false);
        delay = false;
        input.actions.FindActionMap("General").Enable();
        input.actions.FindActionMap("Combat").Disable();
        playerStats.SetStat("currentHealth", playerStats.GetStat("health"));
        tankStats.SetStat("currentHealth", tankStats.GetStat("health"));
        supportStats.SetStat("currentHealth", supportStats.GetStat("health"));
        combatUI.SetFinished(false);
        tempCur = 0;
        inputDetect.t = false;
        objective = "bruh";
        abilityCount = 0;
        turnCount = 0;
    }
    public void UpdateEnemyCount()
    {
        enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyStats>();
    }
    public bool CheckCombatState()
    {
        return isInCombat;
    }
    public void SetCombatState(bool check)
    {
        isInCombat = check;
    }
    public bool CheckEnemyState()
    {
        return enemySpawn;
    }
    public void SetEnemyState(bool check)
    {
        enemySpawn = check;
    }
    public bool CheckMoveState()
    {
        return canMove;
    }
    public void SetMoveState(bool check)
    {
        canMove = check;
    }
    public bool CheckActionState()
    {
        return canAction;
    }
    public void SetActionState(bool check)
    {
        canAction = check;
    }
    public bool CheckAbilityState()
    {
        return canAbility;
    }
    public void SetAbilityState(bool check)
    {
        canAbility = check;
    }
    public bool CheckAttackState()
    {
        return canAttack;
    }
    public void SetAttackState(bool check)
    {
        canAttack = check;
    }
    public bool CheckRange()
    {
        return getRange;
    }
    public void SetRangeState(bool check)
    {
        getRange = check;
    }
    public int CheckCurrentPlayerX()
    {
        return currentPlayerX;
    }
    public void SetCurrentPlayerX(int getX)
    {
        currentPlayerX = getX;
    }
    public int CheckCurrentPlayerY()
    {
        return currentPlayerY;
    }
    public void SetCurrentPlayerY(int getY)
    {
        currentPlayerY = getY;
    }
    public Vector3 CheckCurrentPos()
    {
        return currentPos;
    }
    public void SetCurrentPos(Vector3 pos)
    {
        currentPos = pos;
    }
    public Vector3 GetPlayerPos()
    {
        return player.transform.position;
    }
    public void EndTurn()
    {
        if (turnManager.GetCurrentTurn().CompareTag("Player") && playerAb.GetOverdriveState())
        {
            playerAb.SetOverdriveCooldown(-1);
            if (playerAb.GetOverdriveCooldown() <= 0)
                playerAb.SetOverdriveState(false);
        }
        else if (turnManager.GetCurrentTurn().name == "Tank Ally" && tankAb.GetShieldState())
        {
            tankAb.SetShieldCooldown(-1);
            if (tankAb.GetShieldCooldown() <= 0)
                tankAb.SetShieldState(false);
        }
        if (turnManager.GetCurrentTurn().CompareTag("Player"))
            turnCount += 1;
        combatUI.MainOptions();
        SetActionState(true);
        SetMoveState(false);
        SetAttackState(false);
        SetAbilityState(false);
        combatA.SetMoveArea(false);
        combatA.SetAttackArea(false);

        turnManager.ChangeTurnOrder();
        combatUI.TurnStatus();
    }
    public void BackOption()
    {
        combatA.SetMoveArea(false);
        combatA.SetAttackArea(false);
        SetActionState(true);
        SetMoveState(false);
        SetAttackState(false);
        SetAbilityState(false);
    }
    public void SetDelay()
    {
        delay = true;
    }
    public bool CheckDelay()
    {
        return delay;
    }
    void GetWorldPos()
    {
        worldPos = player.transform.position;
        worldPosRot = player.transform.rotation;
    }
    void SetWorldPos()
    {
        player.transform.position = worldPos;
        player.transform.rotation = worldPosRot;
    }
    public void SetCurrentEnemy(GameObject input)
    {
        currentEnemy = input;
    }
    public GameObject GetCurrentEnemy()
    {
        return currentEnemy;
    }
    void SetSpawns(string objective)
    {
        player.transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        int num = roomDictonary.FirstOrDefault(x => x.Value == objective).Key;
        foreach (GameObject obj in party.GetActiveMembers())
        {
            if (obj.name == "Player")
            {
                tiles = obj.GetComponent<TileMovement>();
                obj.SetActive(true);
                turnManager.AddPlayer(obj);

                obj.transform.position = playerSpawns[num];
                obj.transform.rotation = rotationSpawns[num];
                int x = Mathf.RoundToInt(playerXYSpawns[num].x);
                int y = Mathf.RoundToInt(playerXYSpawns[num].y);
                tiles.SetMoveX(x);
                tiles.SetMoveY(y);

                tiles.UpdateArray();

            }
            else if (obj.name == "Support Ally")
            {
                tiles = obj.GetComponent<TileMovement>();
                obj.SetActive(true);
                turnManager.AddPlayer(obj);

                obj.transform.position = supportSpawns[num];
                obj.transform.rotation = rotationSpawns[num];
                int x = Mathf.RoundToInt(supportXYSpawns[num].x);
                int y = Mathf.RoundToInt(supportXYSpawns[num].y);
                tiles.SetMoveX(x);
                tiles.SetMoveY(y);

                rooms.activeArray[17 - tiles.CheckMoveY(), tiles.CheckMoveX()] = "a" + 1;
                tiles.SetAllyNum("a" + 1);
            }
            else if (obj.name == "Tank Ally")
            {
                tiles = obj.GetComponent<TileMovement>();
                obj.SetActive(true);
                turnManager.AddPlayer(obj);

                obj.transform.position = tankSpawns[num];
                obj.transform.rotation = rotationSpawns[num];
                int x = Mathf.RoundToInt(tankXYSpawns[num].x);
                int y = Mathf.RoundToInt(tankXYSpawns[num].y);
                tiles.SetMoveX(x);
                tiles.SetMoveY(y);

                rooms.activeArray[17 - tiles.CheckMoveY(), tiles.CheckMoveX()] = "a" + 2;
                tiles.SetAllyNum("a" + 2);
            }
        }
    }
    public IEnumerator StartCombatAnim(string enemy)
    {
        float times = 0f;
        Time.timeScale = 0f;
        startCombat.SetActive(true);
        while (times < timer)
        {
            times += Time.unscaledDeltaTime;
            yield return null;
        }
        StartCombat(enemy);
        startCombat.SetActive(false);
        Time.timeScale = 1f;
    }
    public int GetTempCur()
    {
        return tempCur;
    }
    public string GetObjective()
    {
        return objective;
    }
    public int GetTurnCount()
    {
        return turnCount;
    }
    public int GetAbilityCount()
    {
        return abilityCount;
    }
    public void AddAbilityCount()
    {
        abilityCount++;
        Debug.Log(abilityCount);
    }

}

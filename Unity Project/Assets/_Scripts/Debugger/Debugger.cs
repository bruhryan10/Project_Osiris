using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class Debugger : MonoBehaviour
{
    //we do a little bit of trolling in the debugger script
    [SerializeField] CombatActions combatA;
    [SerializeField] CombatManager combatM;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] TurnManager turnManager;
    [SerializeField] EnemyStats enemyStats;
    [SerializeField] PlayerMovement playerScript;
    [SerializeField] ObjectiveManager objectiveM;
    [SerializeField] UpgradeTracker upgrades;
    [SerializeField] NewCombatUI combatUI;

    [SerializeField] Canvas UI;
    public bool debugerEnabled;
    string[] debuggerState = { "select" };
    [SerializeField] TMP_Text debugHeader;
    [SerializeField] TMP_Text combat;
    [SerializeField] TMP_Text select;
    [SerializeField] TMP_Text general;
    [SerializeField] GameObject kill_allEnemy;
    [SerializeField] GameObject leaderEnemy;
    [SerializeField] GameObject poiEnemy;
    [SerializeField] GameObject final_bossEnemy;
    [SerializeField] GameObject Player;
    public bool collision;
    void Start()
    {
        combatUI = GameObject.Find("Combat").GetComponent<NewCombatUI>();
        debugerEnabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
            debugerEnabled = !debugerEnabled;
        if (debugerEnabled)
        {
            DebuggerActions();
            UI.enabled = true;
            Debug.developerConsoleVisible = true;
        }
        else
        {
            Debug.developerConsoleVisible = false;
            UI.enabled = false;

        }

    }
    public void DebuggerActions()
    {
        if (debuggerState[0] == "select")
        {
            debugHeader.text = "DEBUGGER SELECT:";
            combat.enabled = false;
            select.enabled = true;
            general.enabled = false;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                debuggerState[0] = "general";
            if (combatM.CheckCombatState() && Input.GetKeyDown(KeyCode.Alpha2))
                debuggerState[0] = "wombat";
        }
        if (debuggerState[0] == "general")
            DebuggerGeneral();
        if (debuggerState[0] == "wombat")
            DebuggerCombat();
        if (playerScript.move && debuggerState[0] == "wombat")
            debuggerState[0] = "select";
    }
    public void DebuggerGeneral()
    {
        debugHeader.text = "DEBUGGER GENERAL:";
        combat.enabled = false;
        select.enabled = false;
        general.enabled = true;
        if (Input.GetKeyDown(KeyCode.Tab))
            debuggerState[0] = "select";
            string currentScene = SceneManager.GetActiveScene().name;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                SceneManager.LoadScene(currentScene);
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                collision = !collision;
                Rigidbody rb = Player.GetComponent<Rigidbody>();
                Collider collider = Player.GetComponent<Collider>();

                rb.useGravity = !rb.useGravity;
                collider.enabled = !collider.enabled;
            }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerStats = Player.GetComponent<PlayerStats>();
            playerStats.SetCurrency(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerStats = Player.GetComponent<PlayerStats>();
            playerStats.SetCurrency(-10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Vector3 spawnPosition = Player.transform.position;
                Quaternion spawnRotation = Player.transform.rotation;

                // Offset the spawn position to be behind the player's spawn location
                spawnPosition += Player.transform.forward * 10;

                // Instantiate enemy at the calculated position with the spawn rotation
                GameObject enemy = Instantiate(kill_allEnemy, spawnPosition, spawnRotation);
                enemy.name = kill_allEnemy.name;
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Vector3 spawnPosition = Player.transform.position;
                Quaternion spawnRotation = Player.transform.rotation;

                // Offset the spawn position to be behind the player's spawn location
                spawnPosition += Player.transform.forward * 10;

                // Instantiate enemy at the calculated position with the spawn rotation
                GameObject enemy = Instantiate(leaderEnemy, spawnPosition, spawnRotation);
                enemy.name = leaderEnemy.name;
            }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Vector3 spawnPosition = Player.transform.position;
            Quaternion spawnRotation = Player.transform.rotation;

            // Offset the spawn position to be behind the player's spawn location
            spawnPosition += Player.transform.forward * 10;

            // Instantiate enemy at the calculated position with the spawn rotation
            GameObject enemy = Instantiate(poiEnemy, spawnPosition, spawnRotation);
            enemy.name = poiEnemy.name;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Vector3 spawnPosition = new Vector3(Player.transform.position.x, Player.transform.position.y + 30, Player.transform.position.z);
            Quaternion spawnRotation = Player.transform.rotation;

            // Offset the spawn position to be behind the player's spawn location
            spawnPosition += Player.transform.forward * 10;

            // Instantiate enemy at the calculated position with the spawn rotation
            GameObject enemy = Instantiate(final_bossEnemy, spawnPosition, spawnRotation);
            enemy.name = final_bossEnemy.name;
        }

    }
    public void DebuggerCombat()
    {
        if (!combatM.CheckCombatState())
            return;
        debugHeader.text = "DEBUGGER COMBAT:";
        combat.enabled = true;
        select.enabled = false;
        general.enabled = false;
        if (Input.GetKeyDown(KeyCode.Tab))
            debuggerState[0] = "select";
            GameObject currentObj = turnManager.GetCurrentTurn();
            if (currentObj.CompareTag("Player") || currentObj.CompareTag("Ally"))
                playerStats = currentObj.GetComponent<PlayerStats>();
            if (currentObj.CompareTag("Enemy"))
                enemyStats = currentObj.GetComponent<EnemyStats>();
            if (Input.GetKeyDown(KeyCode.Alpha3))
                combatM.EndTurn();
            if (Input.GetKeyDown(KeyCode.Alpha4))
                combatUI.EndCombatAnim();
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                playerStats.IncreaseStat("currentHealth", 999);
                playerStats.IncreaseStat("health", 999);
                playerStats.IncreaseStat("damage", 999);
                playerStats.IncreaseStat("luck", 999);
                playerStats.IncreaseStat("range", 999);
                playerStats.IncreaseStat("speed", 999);
                playerStats.IncreaseStat("summon", 999);
                playerStats.IncreaseStat("siphon", 999);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                playerStats.IncreaseStat("currentHealth", -999);
                playerStats.IncreaseStat("health", -999);
                playerStats.IncreaseStat("damage", -999);
                playerStats.IncreaseStat("luck", -999);
                playerStats.IncreaseStat("range", -999);
                playerStats.IncreaseStat("speed", -999);
                playerStats.IncreaseStat("summon", -999);
                playerStats.IncreaseStat("siphon", -999);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                if (currentObj.CompareTag("Enemy"))
                    enemyStats.DecreaseHealth(enemyStats.GetHealth());
                else
                    playerStats.IncreaseStat("currentHealth",-playerStats.GetStat("currentHealth"));
            }
        if (Input.GetKeyDown(KeyCode.Alpha8))
            objectiveM.DebuggerSet(1);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            objectiveM.DebuggerSet(2);
        if (Input.GetKeyDown(KeyCode.Alpha0))
            objectiveM.DebuggerSet(3);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            upgrades = GameObject.Find("SafeRoomManager").GetComponent<UpgradeTracker>();
            upgrades.AddPlayerAbility(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            upgrades = GameObject.Find("SafeRoomManager").GetComponent<UpgradeTracker>();
            upgrades.AddPlayerAbility(-1);
        }
    }
}

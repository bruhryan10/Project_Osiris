using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class EnemyMovement : MonoBehaviour
{

    [Header("Script References")]
    CombatManager combatM;
    CombatRooms rooms;
    TileMovement tiles;
    PlayerStats stats;
    CombatAttack attack;
    CombatAttack combatAT;
    CombatActions combatACT;
    EnemyStats enemyS;
    EnemyManager enemyM;
    TurnManager turnManager;
    PartyManager partyM;
    EnemyRays enemyRays;

    public Vector3 direction;
   [SerializeField] int enemyX;
    [SerializeField] int enemyY;
    Vector3 enemyPos;
    bool rngMove;
    bool getArray;
    List<GameObject> activeMembers;
    GameObject[] activeMembersArray;
    public GameObject nearestPartyMember;
    public float moveTimer;
    public float moveDelay;
    public bool blockLeft = false;
    public bool blockRight = false;
    public bool blockUp = false;
    public bool blockDown = false;
    public bool isInRange;

    [SerializeField] GameObject Player;


    [SerializeField] int currentMoves = 0;


    void Start()
    {
        combatAT = GameObject.Find("Player").GetComponent<CombatAttack>();
        combatACT = GameObject.Find("Player").GetComponent<CombatActions>();
        combatM = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        rooms = GameObject.Find("CombatManager").GetComponent<CombatRooms>();
        turnManager = GameObject.Find("CombatManager").GetComponent<TurnManager>();
        tiles = GameObject.Find("Player").GetComponent<TileMovement>();
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        attack = GameObject.Find("Player").GetComponent<CombatAttack>();
        enemyM = GameObject.Find("EnemySpawner").GetComponent<EnemyManager>();
        partyM = GameObject.Find("Party Members").GetComponent<PartyManager>();
        enemyS = this.GetComponent<EnemyStats>();
        enemyRays = this.GetComponent<EnemyRays>();
        getArray = true;
        Player = GameObject.Find("Player");
        activeMembers = partyM.GetActiveMembers();
        activeMembersArray = activeMembers.ToArray();
        nearestPartyMember = activeMembersArray.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).FirstOrDefault();
        //rngMove = Random.Range(0f, 100.0f) <= 20f;
        isInRange = false;
    }

    void Update()
    {
        if (combatM.CheckCombatState())
        {
            if (getArray)
                GetArray();
            if (turnManager.GetCurrentTurn() == this.gameObject && !enemyS.CheckAttackStatus())
            {
                if (Regex.IsMatch(gameObject.name, @"^Defender\sEnemy$"))
                {
                    if (moveTimer <= 0f && enemyRays.sightedPartyMember)
                    {
                        Move();
                        moveTimer = moveDelay;
                    }
                    else
                        moveTimer -= Time.deltaTime;
                }
                else if (Regex.IsMatch(gameObject.name, @"^Repair\sEnemy$"))
                {
                    if (moveTimer <= 0f && enemyS.GetHealth() < 6f)
                    {
                        Move();
                        moveTimer = moveDelay;
                    }
                    else
                        moveTimer -= Time.deltaTime;
                }
                else if (Regex.IsMatch(gameObject.name, @"^Boss\sEnemy$"))
                {
                    if (moveTimer <= 0f && enemyS.GetHealth() < 6f)
                    {
                        Move();
                        moveTimer = moveDelay;
                    }
                    else
                        moveTimer -= Time.deltaTime;
                }
                else 
                {
                    if (moveTimer <= 0f)
                    {
                        Move();
                        moveTimer = moveDelay;
                    }
                    else
                        moveTimer -= Time.deltaTime;
                }
            }
        }
    }
    public void MoveLeft()
    {
        //Debug.Log("left");
        enemyX -= 1; //changes the Y in the array space
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + tiles.CheckMoveNum()); //moves the player in the worldspace
        this.currentMoves++;
        //Debug.Log(enemyX + "," + enemyY);
    }
    public void MoveRight()
    {
        //Debug.Log("right");
        enemyX += 1; //changes the Y in the array space
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - tiles.CheckMoveNum()); //moves the player in the worldspace
        this.currentMoves++;
        //Debug.Log(enemyX + "," + enemyY);

    }
    public void MoveDown()
    {
        Debug.Log("DOWN:" + rooms.activeArray[17 - enemyY - 1, enemyX]);
        // Debug.Log("up");
        enemyY -= 1; //changes the Y in the array space
        transform.position = new Vector3(transform.position.x - tiles.CheckMoveNum(), transform.position.y, transform.position.z); //moves the player in the worldspace
        this.currentMoves++;
        //Debug.Log(enemyX + "," + enemyY);

    }
    public void MoveUp()
    {
        Debug.Log("UP:" + rooms.activeArray[17 - enemyY + 1, enemyX]);
        // Debug.Log("down");
        enemyY += 1; //changes the Y in the array space
        transform.position = new Vector3(transform.position.x + tiles.CheckMoveNum(), transform.position.y, transform.position.z); //moves the player in the worldspace
        this.currentMoves++;
       // Debug.Log(enemyX+ "," + enemyY);

    }
    public void GetEnemyPos(Vector3 arrayPos)
    {
        enemyPos = this.GetComponent<EnemyStats>().worldPos;
        enemyY = Mathf.RoundToInt(arrayPos.y);
        enemyX = Mathf.RoundToInt(arrayPos.x);
        //Debug.Log("bruh");
    }
    public void GetArray()
    {
        Vector3 arrayPos = this.GetComponent<EnemyStats>().arrayPos;
        enemyPos = this.GetComponent<EnemyStats>().worldPos;
        GetEnemyPos(arrayPos);
        getArray = false;
    }
    public void UpdateNewArray()
    {
        rooms.activeArray[17 - enemyY, enemyX] = "e";
    }
    public void UpdateOldArray()
    {
        rooms.activeArray[17 - enemyY, enemyX] = "#";
        //Debug.Log("NOT");
    }
    public void Move()
    {
        if (nearestPartyMember.GetComponent<PlayerStats>().GetStat("currentHealth") <= 0)
        {
            activeMembers = partyM.GetActiveMembers();
            activeMembersArray = activeMembers.ToArray();
            nearestPartyMember = activeMembersArray.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).FirstOrDefault();
        }
        direction = transform.position - nearestPartyMember.transform.position;
        direction = Quaternion.Inverse(nearestPartyMember.transform.rotation) * direction;
        //Debug.Log(enemyY);
        //Debug.Log(direction);
        string left = rooms.activeArray[17 - enemyY, enemyX - 1];
        string right = rooms.activeArray[17 - enemyY, enemyX + 1];
        string up = rooms.activeArray[17 - enemyY - 1, enemyX];
        string down = rooms.activeArray[17 - enemyY + 1, enemyX];
        UpdateOldArray();
        if (!Regex.IsMatch(gameObject.name, @"^Boss\sEnemy$"))
        {
            if (!(enemyX + 1 >= rooms.activeArray.Length / rooms.activeArray.GetLength(0)) && direction.z < 0 && !(right == "|" || right == "e" || right == "p" || right[0] == 'a') && !blockRight)
                MoveRight();
            else if (!(enemyX - 1 < 0) && direction.z > 0 && !(left == "|" || left == "e" || left == "p" || left[0] == 'a') && !blockLeft)
                MoveLeft();
            else if (direction.x > 0 && !(up == "|" || up == "e" || up == "p" || up[0] == 'a') && !blockUp)
                MoveUp();
            else if (direction.x < 0 && !(down == "|" || down == "e" || down == "p" || down[0] == 'a') && !blockDown)
                MoveDown();
            else if (blockRight)
                MoveLeft();
            else if (blockLeft)
                MoveRight();
            else if (blockUp)
                MoveDown();
            else if (blockDown)
                MoveUp();
            else
                this.currentMoves++;
            //Debug.Log(rooms.activeArray.GetLength(0));
            //Debug.Log(rooms.activeArray.GetLength(1));
            UpdateNewArray();
/*          for (int i = 0; i < rooms.activeArray.GetLength(0) - 1; i++)
            {
                string brug = "";
                for (int j = 0; j < rooms.activeArray.GetLength(1); j++)
                {
                    brug += rooms.activeArray[i, j];
                }
                Debug.Log(brug);
            }*/
        }

    }
    public int GetCurrentSpeed()
    {
        return currentMoves;
    }
    public void ResetSpeed()
    {
        currentMoves = 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] CombatManager combatM;
    [SerializeField] CombatActions combatA;
    [SerializeField] CombatRooms rooms;
    [SerializeField] EnemyMovement enemyM;
    [SerializeField] CombatActions combatACT;
    [SerializeField] TurnManager turnManager;
    [SerializeField] NewCombatUI combatUI;
    [SerializeField] ObjectiveManager objectiveM;
    [SerializeField] CombatCamera combatCamera;
    EnemyStats enemyS;

    
    
    [SerializeField] GameObject normalPrefab;
    [SerializeField] GameObject leaderPrefab;
    [SerializeField] GameObject final_boss;
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();
    bool hasSpawned = false;
    Vector3 spawnPos;
    [SerializeField] List<int> random;

    [SerializeField] List<Vector3> leaderSpawnPos;
    [SerializeField] List<Vector2> leaderXY;
    [SerializeField] List<Vector3> kill_allSpawnPos;
    [SerializeField] List<Vector2> kill_allXY;
    [SerializeField] List<Vector3> poiSpawnPos;
    [SerializeField] List<Vector2> poiXY;
    bool bruh;


    void Update()
    {
        if (combatM.CheckCombatState())
        {
            CheckCombat();
        }

    }
    public void SpawnEnemy(string objective)
    {
        bruh = false;
        if (objective == "KILL_ALL")
        {
            EnemySpawn(kill_allXY[0], kill_allSpawnPos[0], normalPrefab);
            EnemySpawn(kill_allXY[1], kill_allSpawnPos[1], normalPrefab);
            EnemySpawn(kill_allXY[2], kill_allSpawnPos[2], normalPrefab);

        }
        if (objective == "LEADER")
        {
            EnemySpawn(leaderXY[0], leaderSpawnPos[0], leaderPrefab);
            EnemySpawn(leaderXY[1], leaderSpawnPos[1], normalPrefab);
            EnemySpawn(leaderXY[2], leaderSpawnPos[2], normalPrefab);
            EnemySpawn(leaderXY[3], leaderSpawnPos[3], normalPrefab);
        }
        if (objective == "POI")
        {
            EnemySpawn(poiXY[0], poiSpawnPos[0], normalPrefab);
            EnemySpawn(poiXY[1], poiSpawnPos[1], normalPrefab);
            EnemySpawn(poiXY[2], poiSpawnPos[2], normalPrefab);
        }
/*        if (objective == "FINAL_BOSS")
        {
            enemyList.Add(final_boss);
            turnManager.AddEnemy(final_boss);
            combatM.UpdateEnemyCount();
        }*/
        combatM.SetEnemyState(false);
        hasSpawned = true;
    }
    void CheckCombat()
    {
        foreach (GameObject enemy in enemyList)
        {
            float currentSpeed = enemy.GetComponent<EnemyMovement>().GetCurrentSpeed();
            enemyS = enemy.GetComponent<EnemyStats>();
            int enemyLeft = enemyList.Count;
            if (currentSpeed > 0)
                enemyS.SetMoveStatus(true);
            if (currentSpeed == 4)
            {
                enemyLeft--;
                EndTurn();
            }
            if (enemyLeft == 0)
            {
                //Debug.Log("hello");
                EndTurn();
            }
            if (enemy.name == "Leader Enemy" && turnManager.GetCurrentTurn() == enemy)
                EndTurn();
        }
        if (combatM.GetObjective() == "KILL_ALL" && enemyList.Count == 0 && !bruh)
        {
            combatUI.EndCombatAnim();
            bruh = true;
            Debug.Log("END COMBAT!!!! YIPEEEEE");
        }
        if (combatM.GetObjective() == "LEADER" && GameObject.Find("Leader Enemy") == null && !bruh)
        {
            combatUI.EndCombatAnim();
            bruh = true;
            Debug.Log("END COMBAT!!!! YIPEEEEE");
        }
        if (combatM.GetObjective() == "POI" && GameObject.Find("POICUBE").GetComponent<EnemyStats>().GetHealth() <= 0 && !bruh)
        {
            combatUI.EndCombatAnim();
            bruh = true;
            Debug.Log("END COMBAT!!!! YIPEEEEE");
        }
        if (combatM.GetObjective() == "FINAL_BOSS" && GameObject.Find("Boss Enemy").GetComponent<EnemyStats>().GetHealth() <= 0 && !bruh)
        {
            SceneManager.LoadScene("GameEnd");
            bruh = true;
            Debug.Log("END COMBAT!!!! YIPEEEEE");
        }
    }
    public void EndTurn()
    {
        GameObject enemy = turnManager.GetCurrentTurn();
        if (enemy.CompareTag("Enemy"))
        {
            Debug.Log("End Turn!");
            enemy.GetComponent<EnemyMovement>().ResetSpeed();
            enemy.GetComponent<EnemyStats>().SetAttackStatus(false);
            enemy.GetComponent<EnemyStats>().SetMoveStatus(false);
            turnManager.ChangeTurnOrder();
            combatUI.TurnStatus();
        }
    }
    public void EnemySpawn(Vector2 arrayPos, Vector3 spawnPost, GameObject spawn)
    {
        GameObject spawnObj = Instantiate(spawn, spawnPost, Quaternion.identity);
        enemyList.Add(spawnObj);
        turnManager.AddEnemy(spawnObj);
        spawnObj.GetComponent<EnemyStats>().arrayPos = arrayPos;
        spawnObj.name = "Enemy";
        UpdateR1F1(arrayPos, "e");
        combatM.UpdateEnemyCount();
        if (spawn == leaderPrefab)
        {
            spawnObj.name = "Leader Enemy";
            turnManager.RemoveEnemy(spawnObj);
        }

    }
    void UpdateR1F1(Vector2 arrayPos, string newValue)
    {
        int y = Mathf.RoundToInt(arrayPos.y);
        int x = Mathf.RoundToInt(arrayPos.x);
        rooms.activeArray[17 - y, x] = newValue;
    }
    public void EnemyDeath()
    {
        ResetArray();
        combatM.UpdateEnemyCount();
        Debug.Log("ahhh");
    }
    public void ResetArray()
    {
        for (int i = 0; i < rooms.activeArray.GetLength(0); i++)
            for (int j = 0; j < rooms.activeArray.GetLength(1); j++)
                if (rooms.activeArray[i, j] == "e" || rooms.activeArray[i, j] == "p")
                    rooms.activeArray[i, j] = "#";
    }
    public bool GetHasSpawned()
    {
        return hasSpawned;
    }
    public void SetHasSpawned(bool check)
    {
        hasSpawned = check;
    }
    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }

}

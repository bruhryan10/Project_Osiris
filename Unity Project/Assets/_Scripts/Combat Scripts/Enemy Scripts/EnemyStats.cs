using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

public class EnemyStats : MonoBehaviour
{
    [Header("Script References")]
    EnemyManager enemyM;
    [SerializeField] CombatAttack attack;
    PlayerStats stats;
    CombatManager combatM;
    EnemyMovement enemyMove;
    TurnManager turnManager;
    NewCombatUI combatUI;
    EnemyAttack enemyAttack;

    [Header("Enemy Stats")]
    [SerializeField] float speed = 5;
    [SerializeField] float range = 2;
    [SerializeField] float damage = 2;
    [SerializeField] float health = 6;
    [SerializeField] float luck = 1;

    public Vector2 arrayPos;
    public Vector3 worldPos;
    public bool takeDamage;
    public bool hasBeenAttacked;
    [SerializeField] bool hasAttacked;
    [SerializeField] bool hasMoved;

    bool inRageMode;
    void Start()
    {
        enemyM = GameObject.Find("EnemySpawner").GetComponent<EnemyManager>();
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        combatM = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        enemyMove = GameObject.FindWithTag("Enemy").GetComponent<EnemyMovement>();
        turnManager = GameObject.Find("CombatManager").GetComponent<TurnManager>();
        attack = GameObject.Find("CombatManager").GetComponent<CombatAttack>();
        combatUI = GameObject.Find("Combat").GetComponent<NewCombatUI>();
        enemyAttack = GetComponent<EnemyAttack>();
        hasAttacked = false;
        hasMoved = false;
        hasBeenAttacked = false;
        inRageMode = false;
    }

    public void TakeDamage(float damage, bool repairMode, bool crit)
    {
        if (inRageMode)
            damage *= 0.5f;

        if (!repairMode)
            hasBeenAttacked = true;
        GameObject currentObj = turnManager.GetCurrentTurn();
        Debug.Log(currentObj);
        DecreaseHealth(damage);
        combatUI.ShowDamage(this.gameObject, damage, crit);
        takeDamage = false;
        attack.AfterAttack();
    }
    void Update()
    {

        if (!hasAttacked)
            enemyAttack.canRangedAttack = true;

        if (Regex.IsMatch(gameObject.name, @"^Boss\sEnemy$") && GetHealth() <= 22 && !inRageMode)
        {
            RageMode();
        }

        if (combatM.CheckCombatState() && this.name == "POICUBE" && health <= 0)
        {
            Debug.Log("POI DEATH!");
        }
        else if (health <= 0 || !combatM.CheckCombatState() && this.name != "Boss Enemy")
        {
            combatUI.ShowTurnOrder(false);
            enemyM.GetEnemyList().Remove(this.gameObject);
            turnManager.RemoveEnemy(this.gameObject);
            enemyM.EnemyDeath();
            Destroy(this.gameObject);
            Debug.Log("ahh imd dead");
            combatUI.TurnStatus();
        }
        worldPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    void RageMode()
    {
        speed *= 2;
        range *= 2;
        damage *= 1.5f;
        luck *= 100f; // Absolutely useless luck c:
        inRageMode = true;
    }

    public void IncreaseSpeed(int increase)
    {
        speed += increase;
    }
    public void DecreaseSpeed(int decrease)
    {
        speed -= decrease;
    }
    public void IncreaseRange(int increase)
    {
        range += increase;
    }
    public void DecreaseRange(int decrease)
    {
        range -= decrease;
    }
    public void IncreaseDamage(float increase)
    {
        damage += increase;
    }
    public void DecreaseDamage(float decrease)
    {
        damage -= decrease;
    }
    public void IncreaseHealth(float increase)
    {
        health += increase;
    }
    public void DecreaseHealth(float decrease)
    {
        health -= decrease;
    }
    public void IncreaseLuck(int increase)
    {
        luck += increase;
    }
    public void DecreaseLuck(int decrease)
    {
        luck -= decrease;
    }
    public float GetSpeed()
    {
        return speed;
    }
    public float GetRange()
    {
        return range;
    }
    public float GetDamage()
    {
        return damage;
    }
    public float GetHealth()
    {
        return health;
    }
    public float GetLuck()
    {
        return luck;
    }
    public bool CheckAttackStatus()
    {
        return hasAttacked;
    }
    public void SetAttackStatus(bool input)
    {
        hasAttacked = input;
    }
    public bool CheckMoveStatus()
    {
        return hasMoved;
    }
    public void SetMoveStatus(bool input)
    {
        hasMoved = input;
    }
}

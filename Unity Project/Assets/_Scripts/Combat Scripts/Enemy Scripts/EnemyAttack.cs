using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public bool canAttack;
    public bool canRangedAttack;
    EnemyStats obeliskStats;

    EnemyStats enemyS;
    TurnManager turnManager;
    EnemyMovement enemyM;
    EnemyRays enemyRays;
    [SerializeField] EnemyManager enemyManger;
    NewCombatUI UI;
    [SerializeField]CombatManager combatM;
    PartyManager partyManager;


    // Start is called before the first frame update
    void Start()
    {
        partyManager = GameObject.Find("Party Members").GetComponent<PartyManager>();
        turnManager = GameObject.Find("CombatManager").GetComponent<TurnManager>();
        enemyManger = GameObject.Find("EnemySpawner").GetComponent<EnemyManager>();
        combatM = GameObject.Find("CombatManager").GetComponent<CombatManager>();

        UI = GameObject.Find("Combat").GetComponent<NewCombatUI>();
        enemyS = this.GetComponent<EnemyStats>();
        enemyM = this.GetComponent<EnemyMovement>();
        enemyRays = this.GetComponent<EnemyRays>();
        
    }

    // Update is called once per frame
    void Update()
    {
        combatM = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        if (combatM.GetObjective() == "POI")
            obeliskStats = GameObject.Find("POICUBE").GetComponent<EnemyStats>();
        if (Regex.IsMatch(gameObject.name, @"^Repair\sEnemy$"))
        {
            if (canAttack == true && enemyS.hasBeenAttacked && turnManager.GetCurrentTurn() == this.gameObject && !enemyS.CheckAttackStatus() && !enemyS.CheckMoveStatus())
            {
                Attack();
                canAttack = false;
            }
            else if (!enemyS.hasBeenAttacked && turnManager.GetCurrentTurn() == this.gameObject && !enemyS.CheckAttackStatus() && !enemyS.CheckMoveStatus())
                RepairObelisk();
        }
        else
        {
            if (canAttack == true && turnManager.GetCurrentTurn() == this.gameObject && !enemyS.CheckAttackStatus() && !enemyS.CheckMoveStatus())
            {
                Attack();
                canAttack = false;
            }
        }
        if (Regex.IsMatch(gameObject.name, @"^Boss\sEnemy$"))
        {
            if (canRangedAttack == true && turnManager.GetCurrentTurn() == this.gameObject && !enemyS.CheckAttackStatus() && !enemyS.CheckMoveStatus())
            {
                RangedAttack();
                canRangedAttack = false;
            }
        }
    }

    void RepairObelisk()
    {
        if (obeliskStats.GetHealth() != 10)
        {
                obeliskStats.TakeDamage(-1, true,false);
                enemyS.TakeDamage(1, true, false);
        }
    }
    void Attack()
    {
        GameObject target = enemyRays.enemyHit.OrderBy(go => go.GetComponent<PlayerStats>().GetStat("currentHealth")).FirstOrDefault();

        GameObject oneHitTarget = enemyRays.enemyHit.FirstOrDefault(go => go.GetComponent<PlayerStats>().GetStat("currentHealth") <= enemyS.GetDamage());
        if (oneHitTarget != null)
            target = oneHitTarget;
        else
        {
            GameObject supportTarget = enemyRays.enemyHit.FirstOrDefault(go => go.name == "Support Ally");
            if (supportTarget != null)
                    target = supportTarget;
            else
                    target = enemyRays.enemyHit.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).FirstOrDefault();
        }

        if (target != null)
        {
            target.GetComponent<PlayerStats>().IncreaseStat("currentHealth", -enemyS.GetDamage());
            UI.ShowDamage(target, enemyS.GetDamage(), false);
            enemyM.ResetSpeed();
            enemyS.SetAttackStatus(true);

            Debug.Log("Enemy Attacked");
            enemyManger.EndTurn();
            }
        }
    void RangedAttack()
    {
        GameObject target = partyManager.GetActiveMembers().OrderBy(go => go.GetComponent<PlayerStats>().GetStat("currentHealth")).FirstOrDefault();

        GameObject oneHitTarget = enemyRays.enemyHit.FirstOrDefault(go => go.GetComponent<PlayerStats>().GetStat("currentHealth") <= enemyS.GetDamage());
        if (oneHitTarget != null)
            target = oneHitTarget;
        else
        {
            GameObject supportTarget = enemyRays.enemyHit.FirstOrDefault(go => go.name == "Support Ally");
            if (supportTarget != null)
                target = supportTarget;
            else
                target = enemyRays.enemyHit.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).FirstOrDefault();
        }

        if (target != null)
        {
            target.GetComponent<PlayerStats>().IncreaseStat("currentHealth", -enemyS.GetDamage());
            UI.ShowDamage(target, enemyS.GetDamage(), false);
            enemyM.ResetSpeed();
            enemyS.SetAttackStatus(true);

            Debug.Log("Enemy Attacked");
            enemyManger.EndTurn();
        }
    }
}
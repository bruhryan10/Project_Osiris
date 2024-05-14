using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class AttackTileCollision : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] List<GameObject> passiveTargets;
    EnemyStats enemyStats;
    [SerializeField] PlayerAbility playerAb;
    [SerializeField] TankAbility tankAb;
    [SerializeField] SupportAbility supportAb;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] AbilityManager abilityM;
    [SerializeField] TurnManager turnM;
    NewCombatUI combatUI;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            target = other.gameObject;
        }
        if (this.name == "PassiveSupport" && (other.CompareTag("Player") || other.CompareTag("Ally")))
        {
            passiveTargets.Add(other.gameObject);
            supportAb.SetPassive(other.gameObject,"add", passiveTargets.Count);
        }
        if (this.name == "PassiveTank" && (other.CompareTag("Player") || other.CompareTag("Ally")))
        {
            passiveTargets.Add(other.gameObject);
            tankAb.SetPassive(other.gameObject, "add", passiveTargets.Count);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            target = null;
        }
        if (this.name == "PassiveSupport" && (other.CompareTag("Player") || other.CompareTag("Ally")))
        {
            passiveTargets.Remove(other.gameObject);
            supportAb.SetPassive(other.gameObject, "remove", passiveTargets.Count);   
        }
        if (this.name == "PassiveTank" && (other.CompareTag("Player") || other.CompareTag("Ally")))
        {
            passiveTargets.Add(other.gameObject);
            tankAb.SetPassive(other.gameObject, "remove", passiveTargets.Count);
        }
    }
    public bool IsTargetNull()
    {
        return target == null;
    }
    public void TargetDamage(float damage)
    {
        if (!IsTargetNull())
        {
            combatUI = GameObject.Find("Combat").GetComponent<NewCombatUI>();
            turnM = GameObject.Find("CombatManager").GetComponent<TurnManager>();
            enemyStats = target.GetComponent<EnemyStats>();
            playerStats = turnM.GetCurrentTurn().GetComponent<PlayerStats>();

            bool criticalHit = false;
            float totalRolls = playerStats.GetStat("luck");

            Debug.Log("total Rolls: " + totalRolls);
            for (float i = 0; i < totalRolls; i++)
            {
                float randomNum = Random.Range(1, 101);
                Debug.Log("gENERATED Num " + randomNum);
                if (randomNum <= 20)
                    criticalHit = true;
            }
            Debug.Log("Crit Hit: " + criticalHit);
            if (criticalHit)
                enemyStats.TakeDamage(damage * 1.5f, false, criticalHit);
            else
                enemyStats.TakeDamage(damage, false, criticalHit);

            target = null;
            if (abilityM.GetActiveAbility() == "attackOne")
            {
                playerAb.ResetAttackAbilityOne();
                playerStats.IncreaseStat("currentHealth", -2);
                combatUI.ShowDamage(this.gameObject.transform.parent.gameObject, 2, false);
            }
            if (abilityM.GetActiveAbility() == "airstrike")
            {
                playerStats.IncreaseStat("currentHealth", -2);
                combatUI.ShowDamage(this.gameObject.transform.parent.gameObject, 2, false);
            }

        }
    }
    public void SetTargetNull()
    {
        target = null;
    }
    public GameObject GetTarget()
    {
        return target;
    }
}

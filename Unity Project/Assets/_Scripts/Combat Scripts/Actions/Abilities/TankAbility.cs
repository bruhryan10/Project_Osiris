using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAbility : MonoBehaviour
{
    [SerializeField] bool passiveAbility;
    [SerializeField] AbilityManager abilityM;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] PlayerStats supportStats;
    [SerializeField] PlayerStats tankStats;
    [SerializeField] CombatManager combatM;
    [SerializeField] NewCombatUI UI;
    [SerializeField] GameObject tank;
    [SerializeField] int shieldCooldown;
    [SerializeField] bool shieldActive;
    public void ConfirmShield()
    {
        if (tankStats.GetStat("currentHealth") <= 2)
            return;
        tankStats.IncreaseStat("currentHealth", -2);
        UI.ShowDamage(tank, 2, false);
        shieldCooldown = 4;
        shieldActive = true;
        Debug.Log("Shield");
        ResetAbility();
        combatM.AddAbilityCount();
        combatM.EndTurn();
    }
    public void ResetAbility()
    {
        abilityM.SetActiveAbility("bruh");
    }
    public void SetPassive(GameObject obj, string input, int total)
    {

        if (input == "add")
            obj.GetComponent<PlayerStats>().IncreaseStat("range", 2);
        else if (input == "remove")
            obj.GetComponent<PlayerStats>().IncreaseStat("range", -2);
        if (total > 0)
            passiveAbility = true;
        else
            passiveAbility = false;
    }
    public bool GetPassive()
    {
        return passiveAbility;
    }
    public bool GetShieldState()
    {
        return shieldActive;
    }
    public void SetShieldCooldown(int input)
    {
        shieldCooldown += input;
    }
    public int GetShieldCooldown()
    {
        return shieldCooldown;
    }
    public void SetShieldState(bool input)
    {
        shieldActive = input;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] PlayerAbility playerAb;
    [SerializeField] SupportAbility supportAb;
    [SerializeField] TankAbility tankAb;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] string[] activeAbility = new string[] { "bruh" };
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void PlayerAbility(string input)
    {
        if (input == "overdrive")
             activeAbility[0] = "overdrive";
        if (input == "attackOne")
            activeAbility[0] = "attackOne";
        if (input == "airstrike")
            activeAbility[0] = "airstrike";
    }
    public string GetActiveAbility()
    {
        return activeAbility[0];
    }
    public void SetActiveAbility(string input)
    {
        activeAbility[0] = input;
    }
    public void SupportAbility(string input)
    {
        if (input == "heal")
            activeAbility[0] = "heal";
    }
    public void TankAbility(string input)
    {
        if (input == "shield")
            activeAbility[0] = "shield";
    }
    public void ResetAbilities()
    {
        if (playerAb.GetOverdriveState())
        {
            playerStats.IncreaseStat("range", -3);
            playerStats.IncreaseStat("damage", -3);
        }
        tankAb.SetShieldCooldown(0);
        tankAb.SetShieldState(false);
        playerAb.SetOverdriveCooldown(0);
        playerAb.SetOverdriveState(false);
        activeAbility[0] = "bruh";
    }
}

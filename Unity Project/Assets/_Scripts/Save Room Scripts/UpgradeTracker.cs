using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTracker : MonoBehaviour
{
    [SerializeField] List<int> upgradesPlayer = new List<int>() { 0, 0, 0, 0, 0 };
    [SerializeField] List<int> upgradesTank = new List<int>() { 0, 0, 0, 0, 0 };
    [SerializeField] List<int> upgradesSupport = new List<int>() { 0, 0, 0, 0, 0 };
    [SerializeField] int playerAbility;
    //                  health = 0
    //                  speed = 1
    //                  damage = 2
    //                  range = 3
    //                  luck = 4
    public int CheckUpgrade(string person, int stat)
    {
        if (person == "player")
            return upgradesPlayer[stat] + 1;
        if (person == "tank")
            return upgradesTank[stat] + 1;
        if (person == "support")
            return upgradesSupport[stat] + 1;
        else
            return 0;
    }
    public void SetUpgrade(string person, int stat)
    {
        if (person == "player")
            upgradesPlayer[stat] += 1;
        if (person == "tank")
            upgradesTank[stat] += 1;
        if (person == "support")
            upgradesSupport[stat] += 1;
    }
    public int GetPlayerAbilities()
    {
        return playerAbility;
    }
    public void AddPlayerAbility(int input)
    {
        playerAbility += input;
    }
}

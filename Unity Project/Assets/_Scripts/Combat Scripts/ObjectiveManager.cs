using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] CombatManager combatM;
    [SerializeField] TMP_Text ob;
    [SerializeField] int objectiveCount;
    [SerializeField] bool secondaryOne;
    [SerializeField] bool secondaryTwo;
    int subObj;

    private void Update()
    {
        int brug = 10 - combatM.GetTurnCount();
        if (subObj == 1 && brug <= 0 || subObj == 2 && combatM.GetAbilityCount() != 0)
            ob.fontStyle = FontStyles.Strikethrough;
        else
            ob.fontStyle = FontStyles.Normal;
        if (subObj == 3 && secondaryOne)
            ob.color = new Color32(97, 217, 103, 255);
        else
            ob.color = Color.black;

    }
    public void UpdateCurrency(string objective)
    {
        if (objective == "KILL_ALL")
        {
            if (objectiveCount == 2)
                playerStats.SetCurrency(6);
            else if (objectiveCount == 1)
                playerStats.SetCurrency(4);
            else
                playerStats.SetCurrency(2);
        }
        if (objective == "LEADER")
        {
            if (objectiveCount == 2)
                playerStats.SetCurrency(9);
            else if (objectiveCount == 1)
                playerStats.SetCurrency(6);
            else
                playerStats.SetCurrency(3);
        }
        if (objective == "POI")
        {
            if (objectiveCount == 2)
                playerStats.SetCurrency(9);
            else if (objectiveCount == 1)
                playerStats.SetCurrency(6);
            else
                playerStats.SetCurrency(3);
        }
    }
    public int MainObjectiveGain(string input)
    {
        if (input == "KILL_ALL")
            return 2;
        else if (input == "LEADER")
            return 3;
        else if (input == "POI")
            return 3;
        else
            return 0;
    }
    public int GetMultiplier()
    {
        if (objectiveCount == 0)
            return 1;
        else if (objectiveCount == 1)
            return 2;
        else if (objectiveCount == 2)
            return 3;
        else
            return 99;
    }
    public string GetSecondaryObjectives()
    {
        int brug = 10 - combatM.GetTurnCount();
        if (subObj == 1)
        {
            if (brug == 1)
                return " - Complete combat within " + brug + " turn";
            else if (brug <= 0)
                return " - Complete combat within 0 turns";
            else
                return " - Complete combat within " + brug + " turns";
        }
        else if (subObj == 2)
            return " - Use no Abilities";
        else if (subObj == 3)
            return " - Attack two enemies at the same time";
        else
            return "bruh";

    }
    public bool GetSecondaryOne()
    {
        return secondaryOne;
    }
    public bool GetSecondaryTwo()
    {
        return secondaryTwo;
    }
    public void ClearStats()
    {
        secondaryOne = false;
        secondaryTwo = false;
        objectiveCount = 0;
    }
    public void RandomSubObjective()
    {
        subObj = Random.Range(1, 4);
        Debug.Log(subObj);
    }
    public void DebuggerSet(int num)
    {
        subObj = num;
    }
    public int GetSecondaryNum()
    {
        return subObj;
    }    
    public void SetSubObjective()
    {
        secondaryOne = true;
        objectiveCount = 1;
    }
}

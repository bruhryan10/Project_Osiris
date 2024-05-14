using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SupportAbility : MonoBehaviour
{
    [SerializeField] bool passiveAbility;
    [SerializeField] GameObject selectedObj;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] PlayerStats supportStats;
    [SerializeField] PlayerStats tankStats;
    [SerializeField] CombatManager combatM;
    [SerializeField] AbilityManager abilityM;
    [SerializeField] PartyManager partyM;
    [SerializeField] GameObject support;
    [SerializeField] NewCombatUI UI;

    public void ChangeSelected(string input)
    {
        List<GameObject> objList = new List<GameObject>(partyM.GetActiveMembers());
        int currentIndex = objList.IndexOf(selectedObj);
        int nextIndex = currentIndex;
        if (selectedObj == null)
            selectedObj = objList[0];
        if (input == "up")
            nextIndex = (currentIndex - 1 + objList.Count) % objList.Count;
        else if (input == "down")
            nextIndex = (currentIndex + 1) % objList.Count;
        selectedObj = objList[nextIndex];
        Debug.Log(selectedObj);
    }
    public void ConfirmHeal()
    {
        if (supportStats.GetStat("currentHealth") <= 1 || selectedObj == null || selectedObj.GetComponent<PlayerStats>().GetStat("currentHealth") == selectedObj.GetComponent<PlayerStats>().GetStat("health"))
            return;
        Debug.Log("Object Health before: " + selectedObj.GetComponent<PlayerStats>().GetStat("currentHealth"));
        supportStats.IncreaseStat("currentHealth", -1);
        UI.ShowDamage(support, 1, false);
        selectedObj.GetComponent<PlayerStats>().IncreaseStat("currentHealth", 2);
        Debug.Log("Object Health after: " + selectedObj.GetComponent<PlayerStats>().GetStat("currentHealth"));
        Debug.Log("Heal");
        ResetAbility();
        combatM.AddAbilityCount();
        combatM.EndTurn();
    }
    public void ResetAbility()
    {
        abilityM.SetActiveAbility("bruh");
        selectedObj = null;
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
    public GameObject GetSelectedObj()
    {
        return selectedObj;
    }
}

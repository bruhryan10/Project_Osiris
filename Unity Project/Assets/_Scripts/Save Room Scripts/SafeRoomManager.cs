using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using Discord;

public class SafeRoomManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] Canvas saveUI;
    [SerializeField] DungeonInputManager inputM;
    [SerializeField] UpgradeTracker skillTree;
    [SerializeField] InputDetection inputDetect;
    PlayerStats playerStats;
    PlayerStats playerCurrency;

    [Header("Main Menu")]
    [SerializeField] GameObject abilityMaster;
    [SerializeField] GameObject skillTreeMaster;
    [SerializeField] GameObject leaveMaster;

    [Header("Player Objects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject supportAlly;
    [SerializeField] GameObject tankAlly;

    [Header("SkillTree Objects")]
    [SerializeField] GameObject charSelect;
    [SerializeField] GameObject skillTreeButtons;
    [SerializeField] GameObject treeInfo;
    [SerializeField] TMP_Text treeText;

    [Header("Tree Upgrade Objects")]
    [SerializeField] GameObject healthButton;
    [SerializeField] GameObject speedButton;
    [SerializeField] GameObject damageButton;
    [SerializeField] GameObject RangeButton;
    [SerializeField] GameObject luckButton;
    [SerializeField] Vector2[] buttonPos;

    [Header("Ability Shop Objects")]
    [SerializeField] GameObject abilityShopButtons;
    [SerializeField] GameObject airStrikeButton;


    [SerializeField] TMP_Text selectedPlayer;
    [SerializeField] TMP_Text selectedStats;
    [SerializeField] TMP_Text currentStats;
    [SerializeField] TMP_Text currencyTotal;

    string[] activePlayer = { "bruh" };
    public bool SafeRoom;

    Dictionary<string, int> upgardeName = new Dictionary<string, int>
    {
        { "health", 0 },
        { "speed", 1 },
        { "damage", 2 },
        { "range", 3 },
        { "luck", 4 }
    };
    void Start()
    {
        treeInfo.SetActive(false);
        skillTreeButtons.SetActive(false);
        charSelect.SetActive(false);
        saveUI.enabled = false;
    }
    public void StartSafeRoom()
    {
        SafeRoom = true;
        inputM.SetActiveInput(false, true);
        saveUI.enabled = true;
        inputM.SetInteractState(false);
        Time.timeScale = 0f;
    }
    public void EndSafeRoom()
    {
        inputDetect.t = false;
        SafeRoom = false;
        inputM.SetActiveInput(true, false);
        Time.timeScale = 1f;
        saveUI.enabled = false;
    }
    public void AbilityShop()
    {
        SetMaster(false,false,false);
        abilityShopButtons.SetActive(true);
    }
    public void AbilityPurchase()
    {
        playerCurrency = player.GetComponent<PlayerStats>();
        if (playerCurrency.GetCurrency() < 3)
            return;
        skillTree.AddPlayerAbility(1);
        airStrikeButton.SetActive(false);
    }
    public void AbilityShopback()
    {
        SetMaster(true, true, true);
        abilityShopButtons.SetActive(false);
    }
    public void SetCharSelect(bool input)
    {
        SetMaster(false, false, false);
        charSelect.SetActive(input);
    }
    public void AfterCharSelect(string input)
    {
        charSelect.SetActive(false);
        skillTreeButtons.SetActive(true);
        SetSkillTreeButtons(input);
        SetActivePlayer(input);
    }
    public void CharBack()
    {
        charSelect.SetActive(true);
        skillTreeButtons.SetActive(false);
        SetActivePlayer("bruh");
    }
    public void SetMaster(bool input1, bool input2, bool input3)
    {
        abilityMaster.SetActive(input1);
        skillTreeMaster.SetActive(input2);
        leaveMaster.SetActive(input3);
    }
    public void TreeBack()
    {
        charSelect.SetActive(false);
        SetMaster(true, true, true);
    }
    public void SetActivePlayer(string input)
    {
        activePlayer = new string[] { input };
    }
    public string GetActivePlayer()
    {
        return activePlayer[0];
    }
    public int GetUpgrades(string input)
    {
        int statNum = upgardeName[input];
        return skillTree.CheckUpgrade(activePlayer[0], statNum);
    }
    public string GetUpgradeCost(string input)
    {
        if (playerCurrency.GetCurrency() >= GetUpgrades(input))
            return "\nHold "+ inputDetect.ConfirmUI() +" for " + inputM.GetTimer().ToString("F1") + "s to Confirm!";
        else
            return "\nYou do not have enough!";
    }
    public void UpdateTreeUI(string input)
    {
        if (input == "health")
                treeText.text = "Gives +1 Health\nCost: " + GetUpgrades("health") + GetUpgradeCost("health");
        if (input == "speed")
            treeText.text = "Gives +1 Speed\nCost: " + GetUpgrades("speed") + GetUpgradeCost("speed");
        if (input == "damage")
            treeText.text = "Gives +1 Damage\nCost: " + GetUpgrades("damage") + GetUpgradeCost("damage");
        if (input == "range")
            treeText.text = "Gives +1 Range\nCost: " + GetUpgrades("range") + GetUpgradeCost("range");
        if (input == "luck")
            treeText.text = "Gives +1 Luck\nCost: " + GetUpgrades("luck") + GetUpgradeCost("luck");
    }
    public void UpdateUI(string input)
    {
        UpdateTreeUI(input);
    }
    public void ConfirmUpgrade(string input)
    {
        if (playerCurrency.GetCurrency() < GetUpgrades(input))
        {
            Debug.Log("not enough money L");
            return;
        }
        if (activePlayer[0] == "player")
            playerStats = player.GetComponent<PlayerStats>();
        if (activePlayer[0] == "tank")
            playerStats = tankAlly.GetComponent<PlayerStats>();
        if (activePlayer[0] == "support")
            playerStats = supportAlly.GetComponent<PlayerStats>();
        playerCurrency.SetCurrency(-GetUpgrades(input));
        int statNum = upgardeName[input];
        skillTree.SetUpgrade(activePlayer[0], statNum);
        playerStats.IncreaseStat(input, 1);
        SetSkillTreeButtons(activePlayer[0]);
        Debug.Log("Confirmed Upgrade of " + input + "!");
    }
    public void SetSkillTreeButtons(string input)
    {
        playerCurrency = player.GetComponent<PlayerStats>();
        currencyTotal.text = "Soul Fragments: " + playerCurrency.GetCurrency();
        if (input == "player")
        {
            playerStats = player.GetComponent<PlayerStats>();
            selectedPlayer.text = "Selected: Player";
            selectedStats.text = "Player Stats:";
            currentStats.text = 
                "Health: " + playerStats.GetStat("health") + 
                "\nSpeed: " + playerStats.GetStat("speed") + 
                "\nDamage: " + playerStats.GetStat("damage") + 
                "\nRange: " + playerStats.GetStat("range") + 
                "\nLuck: " + playerStats.GetStat("luck");
        }
        if (input == "support")
        {
            playerStats = supportAlly.GetComponent<PlayerStats>();
            selectedPlayer.text = "Selected: Support Ally";
            selectedStats.text = "Support Ally Stats:";
            currentStats.text =
                "Health: " + playerStats.GetStat("health") +
                "\nSpeed: " + playerStats.GetStat("speed") +
                "\nDamage: " + playerStats.GetStat("damage") +
                "\nRange: " + playerStats.GetStat("range") +
                "\nLuck: " + playerStats.GetStat("luck");
        }
        if (input == "tank")
        {
            playerStats = tankAlly.GetComponent<PlayerStats>();
            selectedPlayer.text = "Selected: Tank Ally";
            selectedStats.text = "Tank Ally Stats:";
            currentStats.text =
                "Health: " + playerStats.GetStat("health") +
                "\nSpeed: " + playerStats.GetStat("speed") +
                "\nDamage: " + playerStats.GetStat("damage") +
                "\nRange: " + playerStats.GetStat("range") +
                "\nLuck: " + playerStats.GetStat("luck");
        }
    }
    public void EnableInfo(string input)
    {
        inputM.SetTimer(3f);
        treeInfo.SetActive(true);
        RectTransform rect = treeInfo.GetComponent<RectTransform>();
        if (input == "health")
            rect.anchoredPosition = buttonPos[0];
        if (input == "speed")
            rect.anchoredPosition = buttonPos[1];
        if (input == "damage")
            rect.anchoredPosition = buttonPos[2];
        if (input == "range")
            rect.anchoredPosition = buttonPos[3];
        if (input == "luck")
            rect.anchoredPosition = buttonPos[4];
        Debug.Log("Enabled Info!");
    }
    public void DisableInfo()
    {
        treeInfo.SetActive(false);
        Debug.Log("Disabled Info!");
    }
}

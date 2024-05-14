using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UnityEngine.Rendering.HighDefinition.ScalableSettingLevelParameter;
using UnityEditor;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System.Linq;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;


public class NewCombatUI : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] InputDetection inputDetect;
    [SerializeField] CombatAttack combatAT;
    [SerializeField] TurnManager turnM;
    [SerializeField] TileMovement tiles;
    [SerializeField] CombatManager combatM;
    [SerializeField] CombatRooms rooms;
    [SerializeField] PlayerStats stats;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] PlayerAbility playerAb;
    [SerializeField] SupportAbility supportAb;
    [SerializeField] TankAbility tankAb;
    [SerializeField] ObjectiveManager objectiveM;
    [SerializeField] UpgradeTracker upgrades;

    [Header("Canvas Info")]
    [SerializeField] Canvas combatUI;
    [SerializeField] GameObject player;

    [Header("Main Action Buttons")]
    [SerializeField] GameObject moveButton;
    [SerializeField] GameObject abilityButton;
    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject passTurnButton;

    [Header("Ability Buttons")]
    [SerializeField] GameObject healAbility;
    [SerializeField] GameObject overdriveAbility;
    [SerializeField] GameObject attackOneAbility;
    [SerializeField] GameObject shieldAbility;
    [SerializeField] GameObject airstrikeAbility;
    [SerializeField] TMP_Text abilityBack;
    [SerializeField] TMP_Text passiveAbility;

    [Header("Details Text")]
    [SerializeField] TMP_Text moveDeatil;
    [SerializeField] TMP_Text attackDetail;
    [SerializeField] TMP_Text statDetails;
    [SerializeField] TMP_Text statHeader;
    [SerializeField] GameObject statMain;
    [SerializeField] TMP_Text abilityText;
    [SerializeField] GameObject statsM;

    [Header("End Objects")]
    [SerializeField] GameObject AllEnd;
    [SerializeField] TMP_Text mainObjReward;
    [SerializeField] TMP_Text secondaryObjTasks;
    [SerializeField] TMP_Text secondaryObjRewards;
    [SerializeField] TMP_Text totalRewards;
    [SerializeField] TMP_Text continueObj;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject black;

    [Header("Objective Objects")]
    [SerializeField] GameObject objMain;
    [SerializeField] GameObject objSec;
    [SerializeField] TMP_Text mainObjective;
    [SerializeField] TMP_Text secondaryObjective;

    [Header("Turn Status")]
    [SerializeField] TMP_Text turnText;
    [SerializeField] GameObject turnOrderOne;
    [SerializeField] GameObject turnOrderTwo;
    [SerializeField] GameObject turnOrderThree;
    [SerializeField] GameObject turnOrderFour;
    [SerializeField] GameObject turnOrderFive;
    [SerializeField] GameObject turnOrderSix;
    [SerializeField] GameObject tankPassive;
    [SerializeField] GameObject supportPassive;
    [SerializeField] Vector2[] buttonPos;
    Dictionary<int, GameObject> turnNum;
    [SerializeField] GameObject crown;
    bool finished;

    [Header("Damage indicators")]
    [SerializeField] TMP_Text damagePrefab;
    [SerializeField] Camera cam;
    [SerializeField] TMP_Text critText;

    [Header("Turn indicators")]
    [SerializeField] Material inactive;
    [SerializeField] Material active;
    Renderer objRenderer;

    void Start()
    {
        critText.enabled = false;
        SetDictionary();
        ToggleDetail(false);
    }
    void Update()
    {
        if (!combatM.CheckCombatState())
        {
            combatUI.enabled = false;
            return;
        }
        combatUI.enabled = true;
        if (!turnM.GetCurrentTurn().CompareTag("Enemy"))
            StatsText();
        if (combatM.CheckMoveState())
            MoveUI();
        if (combatM.CheckAttackState())
            AttackUI();
        ObjectiveUI(combatM.GetObjective());
    }
    void ObjectiveUI(string objective)
    {
        secondaryObjective.text = objectiveM.GetSecondaryObjectives();
        if (objective == "KILL_ALL")
        {
            mainObjective.GetComponent<RectTransform>().anchoredPosition = new Vector2(1145, 463);
            mainObjective.text = "Kill all enemies!";
        }
        else if (objective == "LEADER")
        {
            mainObjective.GetComponent<RectTransform>().anchoredPosition = new Vector2(1047, 463);
            mainObjective.text = "Kill the enemy leader!";
        }
        else if (objective == "POI")
        {
            mainObjective.GetComponent<RectTransform>().anchoredPosition = new Vector2(1070, 463);
            mainObjective.text = "Corrupt the Obelisk!";
        }
        else if (objective == "FINAL_BOSS")
        {
            mainObjective.GetComponent<RectTransform>().anchoredPosition = new Vector2(1055, 463);
            secondaryObjective.GetComponent<RectTransform>().anchoredPosition = new Vector2(1253, 428);
            int turns = 10 - combatM.GetTurnCount();
            mainObjective.text = "Defeat the final boss!";
            secondaryObjective.text = "Turns Remaining: " + turns;
            secondaryObjective.fontSize = 35;
            Vector2 textSize = secondaryObjective.GetPreferredValues(secondaryObjective.text);
            secondaryObjective.GetComponent<RectTransform>().sizeDelta = textSize;
            objSec.SetActive(false);
        }
    }
    IEnumerator ActiveRenderer()
    {
        yield return new WaitForSeconds(0.01f);
        List<GameObject> members = turnM.TurnMembers();
        for (int i = 0; i < members.Count; i++)
        {
            GameObject member = members[i];
            GameObject turnTile = member.transform.Find("PlayerTile").gameObject;
            objRenderer = turnTile.GetComponent<Renderer>();
            if (member == turnM.GetCurrentTurn())
                objRenderer.material = active;
            else
                objRenderer.material = inactive;
        }
    }
    public void TurnStatus()
    {
        if (endScreen.activeInHierarchy)
            return;
        StartCoroutine(ActiveRenderer());
        secondaryObjective.text = objectiveM.GetSecondaryObjectives();
        if (turnM.GetCurrentTurn().name == "Player")
        {
            tiles = turnM.GetCurrentTurn().GetComponent<TileMovement>();
            stats = turnM.GetCurrentTurn().GetComponent<PlayerStats>();
            turnText.text = "PLAYER\n       PHASE";
            statsM.SetActive(true);
            black.SetActive(true);
            black.GetComponent<UnityEngine.UI.Image>().color = new Color32(117,123,171,105);
            ToggleMain(true);
        }
        else if (turnM.GetCurrentTurn().CompareTag("Ally"))
        {
            tiles = turnM.GetCurrentTurn().GetComponent<TileMovement>();
            stats = turnM.GetCurrentTurn().GetComponent<PlayerStats>();
            ToggleMain(true);
            black.SetActive(true);
            black.GetComponent<UnityEngine.UI.Image>().color = new Color32(117, 123, 171, 105);
        }
        else if (turnM.GetCurrentTurn().CompareTag("Enemy"))
        {
            turnText.text = "ENEMY\n       PHASE";
            ToggleMain(false);
            ToggleDetail(false);
            statsM.SetActive(false);
            black.GetComponent<UnityEngine.UI.Image>().color = new Color32(171, 118, 117, 105);
        }
    }
    void StatsText()
    {
        GameObject currentObj = turnM.GetCurrentTurn();
        stats = currentObj.GetComponent<PlayerStats>();
        statDetails.text = ""+stats.GetStat("currentHealth") +"\n" +stats.GetStat("damage") +"\n"+ stats.GetStat("speed") +"\n"+ stats.GetStat("range") +"\n"+ stats.GetStat("luck");
    }
    void MoveUI()
    {
        ToggleMain(false);
        ToggleDetail(false);
        moveDeatil.enabled = true;
        moveDeatil.text = "Movement: \n " + inputDetect.MoveUI() + " - Movement \n Moves Left: " + tiles.GetCurrentSpeed() + "\n" + inputDetect.ConfirmUI() + " - Confirm\n" + inputDetect.BackUI() + " - Back";
    }
    void AttackUI()
    {
        ToggleMain(false);
        ToggleDetail(false);
        attackDetail.enabled = true;
        attackDetail.text = "Attack: \n" + inputDetect.MoveUI() + " - Move tile \n  Move selection to \n  an enemy to attack \n Moves Left: " + combatAT.GetCurrentRange() + "\n" + inputDetect.ConfirmUI() + " - Confirm\n" + inputDetect.BackUI() + " - Back";
    }
    public void AbilityUI()
    {
        ToggleMain(false);
        ToggleDetail(false);
        RectTransform back = abilityBack.GetComponent<RectTransform>();
        if (turnM.GetCurrentTurn().name == "Player")
        {
            abilityBack.enabled = true;
            overdriveAbility.SetActive(true);
            attackOneAbility.SetActive(true);
            Debug.Log("Update");
            SetButtonPos(overdriveAbility, 1);
            SetButtonPos(attackOneAbility, 2);
            if (upgrades.GetPlayerAbilities() == 3)
            {
                airstrikeAbility.SetActive(true);
                SetButtonPos(airstrikeAbility, 3);
            }
            back.anchoredPosition = buttonPos[upgrades.GetPlayerAbilities() + 1];
        }
        else if (turnM.GetCurrentTurn().name == "Support Ally")
        {
            abilityBack.enabled = true;
            passiveAbility.enabled = true;
            if (supportAb.GetPassive())
                passiveAbility.text = "Passive Ability:\n(ACTIVE)\nNearby allies gain\n+2 Range!";
            else
                passiveAbility.text = "Passive Ability:\n(INACTIVE)\nNearby allies gain\n+2 Range!";

            passiveAbility.GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonPos[2].x, buttonPos[2].y + 45);
            healAbility.SetActive(true);
            SetButtonPos(healAbility, 1);
            back.anchoredPosition = buttonPos[3];

        }
        else if (turnM.GetCurrentTurn().name == "Tank Ally")
        {
            abilityBack.enabled = true;
            passiveAbility.enabled = true;
            shieldAbility.SetActive(true);
            if (tankAb.GetPassive())
                passiveAbility.text = "Passive Ability:\n(ACTIVE)\nNearby allies gain\n+1 Damage!";
            else
                passiveAbility.text = "Passive Ability:\n(INACTIVE)\nNearby allies gain\n+1 Damage!";
            SetButtonPos(shieldAbility, 1);
            back.anchoredPosition = buttonPos[3];
        }
    }
    public void AbilityDetails(string input)
    {
        ToggleDetail(false);
        abilityText.enabled = true;
        if (input == "attackOne")
        {
            if (stats.GetStat("currentHealth") <= 2)
                abilityText.text = "Lightning Blast:\nYou do not have \nenough Health!\n" + inputDetect.BackUI() + " - Back";
            else
                abilityText.text = "Lightning Blast:\n Cost - 2 Health\n Deals 5 Damage\n" + inputDetect.MoveUI() + " - Aim attack\n" + inputDetect.ConfirmUI() + " - Confirm\n" + inputDetect.BackUI() + " - Back";
        }
        else if (input == "overdrive")
        {
            if (stats.GetStat("currentHealth") <= 4)
                abilityText.text = "Overdrive:\nYou do not have \nenough Health!\n" + inputDetect.BackUI() + " - Back";
            else if (playerAb.GetOverdriveState())
                abilityText.text = "Overdrive:\n Currently Active!\n" + inputDetect.BackUI() + " - Back";
            else
             abilityText.text = "Overdrive:\n Temporarily boosts \n Range and Damage\nCost - 4 Health\n" + inputDetect.ConfirmUI() + " - Confirm\n" + inputDetect.BackUI() + " - Back";
        }
        else if (input == "airstrike")
        {
            if (stats.GetStat("currentHealth") <= 2)
                abilityText.text = "Airstrike:\nYou do not have \nenough Health!\n" + inputDetect.BackUI() + " - Back";
            else
                abilityText.text = "Airstrike: \n Move selection to \n an enemy to attack\n" + inputDetect.MoveUI() + " - Move tile \nCost - 2 Health\n" + inputDetect.ConfirmUI() + " - Confirm\n" + inputDetect.BackUI() + " - Back";
        }
        else if (input == "shield")
        {
            if (stats.GetStat("currentHealth") <= 2)
                abilityText.text = "Shield:\nYou do not have \nenough Health!\n" + inputDetect.BackUI() + " - Back";
            else if (tankAb.GetShieldState())
                abilityText.text = "Shield:\n Currently Active!\n" + inputDetect.BackUI() + " - Back";
            else
                abilityText.text = "Shield:\nTemporarily give yourself a \nshield to take less damage!\nCost - 2 Health\n" + inputDetect.ConfirmUI() + " - Confirm\n" + inputDetect.BackUI() + " - Back";
        }
        else if (input == "heal")
        {
            if (stats.GetStat("currentHealth") <= 1)
                abilityText.text = "Heal:\nYou do not have enough \nHealth to use this ability!\n" + inputDetect.BackUI() + " - Back";
            else if (supportAb.GetSelectedObj() ==  null)
                abilityText.text = "Heal:\nHeal a selected ally! \n"+inputDetect.VerticalUI() + " - Change Selection \nSelected: Nobody!\nCost - 1 Health\n"+ inputDetect.BackUI() + " - Back";
            else if (supportAb.GetSelectedObj().GetComponent<PlayerStats>().GetStat("currentHealth") == supportAb.GetSelectedObj().GetComponent<PlayerStats>().GetStat("health"))
                abilityText.text = "Heal:\nHeal a selected ally! \n" + inputDetect.VerticalUI() + " - Change Selection \nSelected: " + supportAb.GetSelectedObj().name + "\nCost - 1 Health \nAlly has full health! \n" + inputDetect.BackUI() + " - Back";
            else
                abilityText.text = "Heal:\nHeal a selected ally! \n" + inputDetect.VerticalUI() + " - Change Selection \nSelected: "+ supportAb.GetSelectedObj().name +"\nCost - 1 Health\n" + inputDetect.ConfirmUI() + " - Confirm\n" + inputDetect.BackUI() + " - Back";
        }
    }

    void ToggleDetail(bool input)
    {
        healAbility.SetActive(input);
        overdriveAbility.SetActive(input);
        attackOneAbility.SetActive(input);
        airstrikeAbility.SetActive(input);
        shieldAbility.SetActive(input);
        endScreen.SetActive(input);
        moveDeatil.enabled = input;
        attackDetail.enabled = input;
        abilityText.enabled = input;
        abilityBack.enabled = input;
        passiveAbility.enabled = input;
        AllEnd.SetActive(input);
    }
    void ToggleMain(bool input)
    {
        moveButton.SetActive(input);
        abilityButton.SetActive(input);
        attackButton.SetActive(input);
        passTurnButton.SetActive(input);
    }
    void SetButtonPos(GameObject button, int index)
    {
        //Debug.Log("setting Pios");
        if (button != null && index >= 0 && index < buttonPos.Length)
        {
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = buttonPos[index];
        }
    }
    public void MainOptions()
    {
        ToggleDetail(false);
        ToggleMain(true);
    }
    public void DefaultState()
    {
        ObjectiveToggle(true);
        turnText.enabled = true;
        statHeader.enabled = true;
        statMain.SetActive(true);
        black.SetActive(true);
        statDetails.enabled = true;
        ToggleDetail(false);
        ToggleMain(true);
    }
    public void EndCombatAnim()
    {
        CheckSub();
        EndToggle();
        objectiveM.UpdateCurrency(combatM.GetObjective());
        AllEnd.SetActive(true);
        endScreen.SetActive(true);
        mainObjReward.text = " +" + objectiveM.MainObjectiveGain(combatM.GetObjective()) +" Soul Fragments";
        secondaryObjTasks.text = objectiveM.GetSecondaryObjectives();
        if (!objectiveM.GetSecondaryOne())
        {
            Debug.Log("one");
            secondaryObjRewards.fontStyle = FontStyles.Strikethrough;
            secondaryObjRewards.color = new Color32(116, 112, 112, 255);
            secondaryObjTasks.fontStyle = FontStyles.Strikethrough;
            secondaryObjTasks.color = new Color32(116, 112, 112, 255);
        }
        else
        {
            Debug.Log("two");

            secondaryObjRewards.fontStyle = FontStyles.Normal;
            secondaryObjRewards.color = new Color32(97, 217, 103, 255);
            secondaryObjTasks.fontStyle = FontStyles.Normal;
            secondaryObjTasks.color = new Color32(97, 217, 103, 255);
        }

        secondaryObjRewards.text = " - 2X Soul Fragments";
        int total = objectiveM.MainObjectiveGain(combatM.GetObjective()) * objectiveM.GetMultiplier();
        totalRewards.text = "Total Rewards: +" + total + " Soul Fragments";
        continueObj.text = "Press " + inputDetect.ConfirmUI() + " to continue!";
        finished = true;
        objectiveM.ClearStats();
        turnM.clearTurnOrder();
        turnM.AddPlayer(player);
    }
    void CheckSub()
    {
        int brug = 10 - combatM.GetTurnCount();
        if (objectiveM.GetSecondaryNum() == 1 && brug >= 0)
            objectiveM.SetSubObjective();
        else if (objectiveM.GetSecondaryNum() == 2 && combatM.GetAbilityCount() == 0)
            objectiveM.SetSubObjective();
    }
    void EndToggle()
    {
        black.SetActive(false);
        ToggleDetail(false);
        ToggleMain(false);
        black.SetActive(false);
        turnText.enabled = false;
        statMain.SetActive(false);
        statHeader.enabled = false;
        statDetails.enabled = false;
        ObjectiveToggle(false);
    }
    void ObjectiveToggle(bool input)
    {
        mainObjective.enabled = input;
        secondaryObjective.enabled = input;
        objMain.SetActive(input);
        objSec.SetActive(input);
    }

    public void ShowTurnOrder(bool input)
    {
        supportPassive.GetComponent<MeshRenderer>().enabled = input;
        tankPassive.GetComponent<MeshRenderer>().enabled = input;
        foreach (GameObject obj in turnM.turnOrder)
        {
            Vector3 pos = obj.transform.position;
            int currentPosition = turnM.turnOrder.IndexOf(obj);
            if (obj.name == "Leader Enemy")
            {
                crown.SetActive(input);
                crown.transform.position = new Vector3(pos.x, pos.y + 1.25f, pos.z);
            }
            //Debug.Log(currentPosition);
            if (turnNum.ContainsKey(currentPosition) && obj.name != "Leader Enemy")
            {
                GameObject turn = turnNum[currentPosition];
                //Debug.Log(turn);
                turn.SetActive(input);
                turn.transform.position = new Vector3(pos.x, pos.y + 1.25f, pos.z);
            }                
        }
        if (GameObject.Find("Leader Enemy") != null)
        {
            GameObject obje = GameObject.Find("Leader Enemy");
            Vector3 pos = obje.transform.position;
            crown.SetActive(input);
            crown.transform.position = new Vector3(pos.x, pos.y + 1.25f, pos.z);
        }
    }
    void SetDictionary()
    {
        turnNum = new Dictionary<int, GameObject>
        {
            { 0, turnOrderOne },
            { 1, turnOrderTwo },
            { 2, turnOrderThree },
            { 3, turnOrderFour },
            { 4, turnOrderFive },
            { 5, turnOrderSix }
        };
    }
    public bool GetFinished()
    {
        return finished;
    }
    public void SetFinished(bool input)
    {
        finished = input;
    }
    public void ShowDamage(GameObject target, float damage, bool critical)
    {
        Vector3 targetPositionScreenSpace = cam.WorldToScreenPoint(target.transform.position);

        TMP_Text damageText = Instantiate(damagePrefab, Vector3.zero, Quaternion.identity);
        RectTransform damageRectTransform = damageText.GetComponent<RectTransform>();

        Vector3 adjustedPosition = new Vector3(targetPositionScreenSpace.x, targetPositionScreenSpace.y + 50f, targetPositionScreenSpace.z);
        damageRectTransform.position = adjustedPosition;

        if (critical)
        {
            Debug.Log("Shpui;d be crit");
            critText.enabled = true;
            Vector3 newPos = new Vector3(targetPositionScreenSpace.x - 50, targetPositionScreenSpace.y + 115f, targetPositionScreenSpace.z);
            critText.GetComponent<RectTransform>().position = newPos;

        }
        damageText.text = "-" + damage.ToString();

        Vector2 textSize = damageText.GetPreferredValues(damageText.text);
        damageRectTransform.sizeDelta = textSize;

        damageText.transform.SetParent(this.transform);
        StartCoroutine(Timer(damageText));
    }
    IEnumerator Timer(TMP_Text damage)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(damage);
        critText.enabled = false;
    }
    public void CameraUI(bool input)
    {
        ToggleMain(input);
        statsM.SetActive(input);
        black.SetActive(input);
        turnText.enabled = input;
    }

}

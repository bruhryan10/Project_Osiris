using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using static UnityEngine.GridBrushBase;

public class CombatActions : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] CombatManager combatM;
    [SerializeField] CombatRooms rooms;
    [SerializeField] TileMovement tiles;
    [SerializeField] ResetSelectedButton resetButton;
    [SerializeField] PlayerStats stats;
    [SerializeField] CombatAttack combatAT;
    [SerializeField] TurnManager turnManager;

    [SerializeField] GameObject moveArea;
    [SerializeField] GameObject attackArea;
    [SerializeField] GameObject camTest;

    void Start()
    {
        attackArea.SetActive(false);
        camTest.SetActive(false);
        moveArea.SetActive(false);
    }

    void Update()
    {
        if (!combatM.CheckCombatState())
            return;
        GameObject currentObj = turnManager.GetCurrentTurn();
        camTest.SetActive(true);
        if (currentObj.CompareTag("Player"))
        {
            SetPlayerRef();
        }
        else if (currentObj.CompareTag("Ally"))
        {
            SetAllyRef();
        }
    }
    public void MoveAction()
    {
        Debug.Log("you can move now!!");
        combatM.SetActionState(false);
        combatM.SetMoveState(true);
        moveArea.SetActive(true);
        combatM.SetCurrentPos(new Vector3(turnManager.GetCurrentTurn().transform.position.x, turnManager.GetCurrentTurn().transform.position.y, turnManager.GetCurrentTurn().transform.position.z));
        combatM.SetCurrentPlayerX(tiles.CheckMoveX());
        combatM.SetCurrentPlayerY(tiles.CheckMoveY());
        tiles.GetArrayPos();
        tiles.GetRot();
        resetButton.ResetButton();
    }
    public void AttackAction()
    {
        Debug.Log("you can use basic attack!!");
        combatM.SetActionState(false);
        combatM.SetAttackState(true);
        combatM.SetRangeState(true);
        combatM.SetCurrentPos(new Vector3(turnManager.GetCurrentTurn().transform.position.x, turnManager.GetCurrentTurn().transform.position.y, turnManager.GetCurrentTurn().transform.position.z));
        combatAT.SetAttackX(tiles.CheckMoveX());
        combatAT.SetAttackY(tiles.CheckMoveY());
        combatAT.GetRot();
        resetButton.ResetButton();
    }
    public void AbilityAction()
    {
        Debug.Log("you can use an ability!!");
        combatM.SetActionState(false);
        combatM.SetAbilityState(true);
        resetButton.ResetButton();
    }
    public GameObject GetAttackArea()
    {
        return attackArea;
    }
    public void SetAttackAreaState(bool state)
    {
        attackArea.SetActive(state);
    }
    public Vector3 GetAttackAreaPosition()
    {
        return new Vector3(attackArea.transform.position.x, attackArea.transform.position.y, attackArea.transform.position.z);
    }
    public void SetAttackAreaPosition(Vector3 pos)
    {
        attackArea.transform.position = pos;
    }
    public void SetAttackArea(bool input)
    {
        attackArea.SetActive(input);
    }
    public void SetMoveArea(bool input)
    {
        moveArea.SetActive(input);
    }
    public void SetCam()
    {
        camTest.SetActive(true);
    }
    public void SetPlayerRef()
    {
        GameObject currentObj = turnManager.GetCurrentTurn();
        tiles = GameObject.Find("Player").GetComponent<TileMovement>();
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        //camTest = currentObj.transform.Find("CombatCam").gameObject;
        moveArea = currentObj.transform.Find("Movement Grid").gameObject;
        attackArea = currentObj.transform.Find("Attack Tile").gameObject;
    }
    public void SetAllyRef()
    {
        GameObject currentObj = turnManager.GetCurrentTurn();
        tiles = currentObj.gameObject.GetComponent<TileMovement>();
        stats = currentObj.gameObject.GetComponent<PlayerStats>();
        //camTest = currentObj.transform.Find("CombatCam").gameObject;
        moveArea = currentObj.transform.Find("Ally Move TIle").gameObject;
        attackArea = currentObj.transform.Find("Attack Tile").gameObject;
    }
}

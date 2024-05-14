using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatInputManager : MonoBehaviour
{
    PlayerInput input;
    InputAction move;
    InputAction back;
    InputAction confirm;
    InputAction turnOrder;
    InputAction pause;
    InputAction right;
    InputAction zoom;
    [SerializeField] GameObject player;
    [SerializeField] NewCombatUI UI;
    [SerializeField] TileMovement tiles;
    [SerializeField] CombatAttack attack;
    [SerializeField] CombatManager combatM;
    [SerializeField] CombatAttack combatAT;
    [SerializeField] CombatCamera combatCam;
    [SerializeField] TurnManager turnM;
    [SerializeField] NewCombatUI combatUI;
    [SerializeField] DeathManager deathM;
    [SerializeField] AbilityManager combatAb;
    [SerializeField] PlayerAbility playerAb;
    [SerializeField] TankAbility tankAb;
    [SerializeField] SupportAbility supportAb;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] PlayerStats playerStats;
    bool isMoving = false;

    void Start()
    {
        input = player.GetComponent<PlayerInput>();
        move = input.actions.FindActionMap("Combat").FindAction("Movement");
        back = input.actions.FindActionMap("Combat").FindAction("Back");
        confirm = input.actions.FindActionMap("Combat").FindAction("Confirm");
        turnOrder = input.actions.FindActionMap("Combat").FindAction("TurnOrder");
        pause = input.actions.FindActionMap("Combat").FindAction("Pause");
        right = input.actions.FindActionMap("Combat").FindAction("right");
        zoom = input.actions.FindActionMap("Combat").FindAction("Zoom");

        move.performed += ctx => MovePlayer(ctx.ReadValue<Vector2>());
        back.performed += ctx => BackButton();
        confirm.performed += ctx => ConfirmButton();
        pause.performed += ctx => PauseGame();
        right.performed += ctx => combatCam.StartMoveCam();
        right.canceled += ctx => combatCam.ResetCam();
        zoom.performed += ctx => Brug(ctx.ReadValue<Vector2>());
    }
    void Brug(Vector2 bruhh)
    {
        //Debug.Log(bruhh);
        combatCam.ZoomCam(bruhh);
    }
    void Update()
    {
        if (!combatM.CheckCombatState() || deathM.GetDeath() || UI.GetFinished())
            return;
        if (turnOrder.IsPressed())
            combatUI.ShowTurnOrder(true);
        else
            combatUI.ShowTurnOrder(false);
        if (right.IsPressed())
            combatCam.MoveCam();
        if (turnM.GetCurrentTurn().CompareTag("Player") || turnM.GetCurrentTurn().CompareTag("Ally"))
            tiles = turnM.GetCurrentTurn().GetComponent<TileMovement>();
    }
    void PauseGame()
    {
        pauseMenu.CheckPause();
    }
    void ConfirmButton()
    {
        if (combatM.CheckMoveState())
            tiles.AfterMoving();
        if (UI.GetFinished())
            combatM.AfterCombat();
        else if (combatM.CheckAttackState())
            combatAT.WithinAttack();
        else if (combatAb.GetActiveAbility() == "attackOne")
            playerAb.ConfirmAttackAbilityOne();
        else if (combatAb.GetActiveAbility() == "overdrive" && !playerAb.GetOverdriveState())
            playerAb.ConfirmOverdrive();
        else if (combatAb.GetActiveAbility() == "shield" && !tankAb.GetShieldState())
            tankAb.ConfirmShield();
        else if (combatAb.GetActiveAbility() == "heal")
            supportAb.ConfirmHeal();
        else if (combatAb.GetActiveAbility() == "airstrike")
            playerAb.AirstrikeConfirm();
        else
            Debug.Log("nothing to confirm");
    }
    void BackButton()
    {
        if (combatM.CheckMoveState())
        {
            tiles.ResetMove();
            UI.MainOptions();
        }
        if (combatM.CheckAttackState())
        {
            combatAT.ResetAttack();
            UI.MainOptions();
        }
        if (combatM.CheckAbilityState())
        {
            if (combatAb.GetActiveAbility() == "attackOne")
            {
                playerAb.ResetAttackAbilityOne();
                UI.AbilityUI();
            }
            else if (combatAb.GetActiveAbility() == "overdrive")
            {
                playerAb.ResetOverdrive();
                UI.AbilityUI();
            }
            else if (combatAb.GetActiveAbility() == "shield")
            {
                tankAb.ResetAbility();
                UI.AbilityUI();
            }
            else if (combatAb.GetActiveAbility() == "heal")
            {
                supportAb.ResetAbility();
                UI.AbilityUI();
            }
            else if (combatAb.GetActiveAbility() == "airstrike")
            {
                playerAb.AirstrikeBack();
            }
            else
            {
                combatM.BackOption();
                Debug.Log("Back ability!");
                UI.MainOptions();
            }
        }
        else
            Debug.Log("nothing to back");
    }
    void MovePlayer(Vector2 dir)
    {
        if (combatM.CheckMoveState())
        {
            if (dir.y > 0 && !isMoving)
            {
                tiles.Controls("up");
                isMoving = true;
            }
            if (dir.y < 0 && !isMoving)
            {
                tiles.Controls("down");
                isMoving = true;
            }
            if (dir.x > 0 && !isMoving)
            {
                tiles.Controls("right");
                isMoving = true;
            }
            if (dir.x < 0 && !isMoving)
            {
                tiles.Controls("left");
                isMoving = true;
            }
        }
        if (combatM.CheckAttackState())
        {
            if (dir.y > 0 && !isMoving)
            {
                attack.Controls("up");
                isMoving = true;
            }
            if (dir.y < 0 && !isMoving)
            {
                attack.Controls("down");
                isMoving = true;
            }
            if (dir.x > 0 && !isMoving)
            {
                attack.Controls("right");
                isMoving = true;
            }
            if (dir.x < 0 && !isMoving)
            {
                attack.Controls("left");
                isMoving = true;
            }
        }
        if (combatAb.GetActiveAbility() == "attackOne")
        {
            if (dir.y > 0 && !isMoving)
            {
                playerAb.AttackAbilityOne("up");
                isMoving = true;
            }
            if (dir.y < 0 && !isMoving)
            {
                playerAb.AttackAbilityOne("down");
                isMoving = true;
            }
            if (dir.x > 0 && !isMoving)
            {
                playerAb.AttackAbilityOne("right");
                isMoving = true;
            }
            if (dir.x < 0 && !isMoving)
            {
                playerAb.AttackAbilityOne("left");
                isMoving = true;
            }
        }
        if (combatAb.GetActiveAbility() == "airstrike")
        {
            if (playerStats.GetStat("currentHealth") <= 2)
                return;
            if (dir.y > 0 && !isMoving)
            {
                playerAb.AirstrikeMove("up");
                isMoving = true;
            }
            if (dir.y < 0 && !isMoving)
            {
                playerAb.AirstrikeMove("down");
                isMoving = true;
            }
            if (dir.x > 0 && !isMoving)
            {
                playerAb.AirstrikeMove("right");
                isMoving = true;
            }
            if (dir.x < 0 && !isMoving)
            {
                playerAb.AirstrikeMove("left");
                isMoving = true;
            }
        }
        if (combatAb.GetActiveAbility() == "heal")
        {
            if (dir.y > 0 && !isMoving)
            {
                supportAb.ChangeSelected("up");
                UI.AbilityDetails("heal");
                isMoving = true;
            }
            if (dir.y < 0 && !isMoving)
            {
                supportAb.ChangeSelected("down");
                UI.AbilityDetails("heal");
                isMoving = true;
            }
        }
        if (isMoving)
            StartCoroutine(ResetMovementFlag());
    }
    IEnumerator ResetMovementFlag()
    {
        yield return new WaitForSeconds(0.1f);

        isMoving = false;
    }
}

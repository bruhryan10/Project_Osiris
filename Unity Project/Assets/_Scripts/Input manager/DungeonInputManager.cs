using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonInputManager : MonoBehaviour
{
    [SerializeField] SafeRoomManager safeRoomM;
    [SerializeField] GameObject player;
    [SerializeField] PauseMenu pauseMenu;
    PlayerInput input;
    InputAction back;
    InputAction confirmObelisk;
    InputAction interact;
    InputAction pause;
    bool canInteract;
    bool canConfirm;
    bool canBack;
    bool getName;
    [SerializeField] float holdTime;
    string[] interactName = { "bruh" };
    string[] confirmName = { "bruh" };
    string[] backName = { "bruh" };
    string[] lastName = { "bruh" };
    void Start()
    {
        input = player.GetComponent<PlayerInput>();
        interact = input.actions.FindActionMap("General").FindAction("Interact");
        confirmObelisk = input.actions.FindActionMap("Obelisk").FindAction("Confirm");
        back = input.actions.FindActionMap("Obelisk").FindAction("Back");
        pause = input.actions.FindActionMap("General").FindAction("Pause");
        pause.performed += ctx => PauseGame();
        interact.performed += ctx => Interact();
        confirmObelisk.performed += ctx => StartConfirm();
        confirmObelisk.canceled += ctx => StopConfirm();
        back.performed += ctx => Back();
    }
    void PauseGame()
    {
        pauseMenu.CheckPause();
    }
    void StartConfirm()
    {
        Debug.Log("start confirm");
        string confirm = confirmName[0];
        Debug.Log("Confirm name " + confirm);
        getName = true;
        if (getName)
        {
            lastName[0] = confirmName[0];
            getName = false;
            Debug.Log("Last Name " + lastName[0]);
        }
        if (confirm == "health" || confirm == "speed" || confirm == "damage" || confirm == "range" || confirm == "luck" || confirm == "bruh")
            holdTime = 3f;
        StartCoroutine(ConfirmCoroutine());
    }
    void StopConfirm()
    {
        Debug.Log("stop confirm");
        string confirm = confirmName[0];
        if (confirm == "health" || confirm == "speed" || confirm == "damage" || confirm == "range" || confirm == "luck" || confirm == "bruh")
            holdTime = 3f;
        StopAllCoroutines();
    }
    IEnumerator ConfirmCoroutine()
    {
        Debug.Log(holdTime);
        while (holdTime > 0f)
        {
            Debug.Log("Last Name " + lastName[0]);
            holdTime -= Time.unscaledDeltaTime;
            //Debug.Log("Hold time: " + holdTime);
            if (confirmName[0] != lastName[0])
                StopConfirm();
            yield return null;
        }
        if (holdTime <= 0f)
            Confirm();

    }
    private void Update()
    {
        string confirm = confirmName[0];
        if (confirm == "health" || confirm == "speed" || confirm == "damage" || confirm == "range" || confirm == "luck")
            safeRoomM.UpdateUI(confirm);
    }
    public void Interact()
    {
        if (!canInteract)
        {
            Debug.Log("NOTHING TO INTERACT!");
            return;
        }
        if (interactName[0] == "Obelisk")
        {
            safeRoomM.StartSafeRoom();
            Debug.Log("OBELISK SAFE ZONE!");
        }
        Debug.Log("INTERACT HAPPENED!");
    }
    public void Confirm()
    {
        Debug.Log("CONFIRM HAPPENED!");
        string confirm = confirmName[0];
        if (confirm == "health" || confirm == "speed" || confirm == "damage" || confirm == "range" || confirm == "luck")
            safeRoomM.ConfirmUpgrade(confirm);

    }
    public void Back()
    {
        Debug.Log("BACK HAPPENED!");
    }
    public void SetActiveInput(bool input1, bool input2)
    {
        if (input1) input.actions.FindActionMap("General").Enable();
        else input.actions.FindActionMap("General").Disable();
        if (input2) input.actions.FindActionMap("Obelisk").Enable();
        else input.actions.FindActionMap("Obelisk").Disable();
    }
    public bool GetInteractState()
    {
        return canInteract;
    }
    public void SetInteractState(bool input)
    {
        canInteract = input;
    }
    public void SetInteractName(string input)
    {
        interactName = new string[] { input };
    }
    public string GetInteractName()
    {
        return interactName[0];
    }
    public bool GetConfirmState()
    {
        return canConfirm;
    }
    public void SetConfirmState(bool input)
    {
        canConfirm = input;
    }
    public void SetConfirmName(string input)
    {
        confirmName = new string[] { input };
    }
    public string GetConfirmName()
    {
        return confirmName[0];
    }
    public bool getBackState()
    {
        return canBack;
    }
    public void SetBackState(bool input)
    {
        canBack = input;
    }
    public void SetBackName(string input)
    {
        backName = new string[] { input };
    }
    public string GetBackName()
    {
        return backName[0];
    }
    public float GetTimer()
    {
        return holdTime;
    }
    public void SetTimer(float input)
    {
        holdTime = input;
    }
}

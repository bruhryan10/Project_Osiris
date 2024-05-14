using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InputDetection : MonoBehaviour
{
    PlayerInput input;
    [SerializeField] GameObject player;
    [SerializeField] DungeonInputManager inputM;
    [SerializeField] CombatManager combatM;
    [SerializeField] SafeRoomManager roomM;
    [SerializeField] CombatCamera combatCam;
    Mouse mouse;
    bool controller;
    bool keyboard;
    [SerializeField] bool hidden;
    public bool t;

    void Start()
    {
        input = player.GetComponent<PlayerInput>();
        mouse = Mouse.current;
        t = false;
        combatCam = GameObject.Find("CombatManager").GetComponent<CombatCamera>();
    }

    void Update()
    {
        CheckCombatStatus();
        CheckControlScheme();
    }
    void CheckCombatStatus()
    {
        Vector2 centerPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
            hidden = !hidden;
        if (roomM.SafeRoom || combatM.CheckCombatState() && !combatCam.GetCameraMoving())
        {
            if (!t)
                mouse.WarpCursorPosition(centerPos);
            t = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (hidden)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    public string ConfirmUI()
    {
        if (controller)
            return "X";
        if (keyboard)
            return "ENTER";
        else
            return "INVALID INPUT!";
    }
    public string VerticalUI()
    {
        if (controller)
            return "D-PAD UP & DOWN";
        if (keyboard)
            return "W, S";
        else
            return "INVALID INPUT!";
    }
    public string BackUI()
    {
        if (controller)
            return "O";
        if (keyboard)
            return "B";
        else
            return "INVALID INPUT!";
    }
    public string MoveUI()
    {
        if (controller)
            return "D-Pad";
        if (keyboard)
            return "WASD";
        else
            return "INVALID INPUT!";
    }
    public void CheckControlScheme()
    {
        controller = GetControlScheme() == "Controller";
        keyboard = GetControlScheme() == "Keyboard";
    }
    public bool GetController()
    {
        return controller;
    }
    public string GetControlScheme()
    {
        return input.currentControlScheme;
    }

}

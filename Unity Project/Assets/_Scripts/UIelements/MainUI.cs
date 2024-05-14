using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] CombatManager combatM;
    [SerializeField] DungeonInputManager interactM;

    [SerializeField] Canvas mainUI;
    [SerializeField] TMP_Text Interact;
    void Start()
    {
        mainUI.enabled = true;
    }

    void Update()
    {
        if (combatM.CheckCombatState())
            mainUI.enabled = false;
        else
            mainUI.enabled = true;

        if (mainUI.enabled)
        {
            if (interactM.GetInteractState())
                Interact.enabled = true;
            else
                Interact.enabled = false;
        }
    }
}

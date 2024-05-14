using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetSelectedButton : MonoBehaviour
{
    [SerializeField]EventSystem eventSystem;
    [SerializeField] GameObject NullObj;
    public void ResetButton()
    {
        eventSystem.SetSelectedGameObject(NullObj);
    }
}

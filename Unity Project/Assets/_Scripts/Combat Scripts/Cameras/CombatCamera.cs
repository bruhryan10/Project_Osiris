using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class CombatCamera : MonoBehaviour
{
    [SerializeField] CombatManager combatM;
    [SerializeField] TurnManager turnM;
    [SerializeField] GameObject combatCam;
    [SerializeField] NewCombatUI combatUI;
    [SerializeField] float rotSpeed = 100f;
    [SerializeField] float transitionDuration;
    Mouse mouse;
    [SerializeField] bool isCameraMoving;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float minFOV = 10f;
    [SerializeField] float maxFOV = 60f;
    Vector3 cameraPos;
    Quaternion cameraRot;
    Vector2 mousePos;
    private Coroutine zoomCoroutine;

    private void Awake()
    {
        mouse = Mouse.current;
        cameraPos = combatCam.transform.position;
        cameraRot = combatCam.transform.rotation;
    }


    public void ZoomCam(Vector2 scrollValue)
    {
        float zoomDelta = scrollValue.y > 0 ? zoomSpeed : -zoomSpeed;

        float targetFOV = Mathf.Clamp(combatCam.GetComponent<Camera>().fieldOfView - zoomDelta, minFOV, maxFOV);

        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ChangeFOVSmoothly(targetFOV));
    }
    private IEnumerator ChangeFOVSmoothly(float targetFOV)
    {
        float newDuration = 0.1f;
        float startFOV = combatCam.GetComponent<Camera>().fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < newDuration)
        {
            elapsedTime += Time.deltaTime;
            combatCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / newDuration);
            yield return null;
        }

        combatCam.GetComponent<Camera>().fieldOfView = targetFOV;
    }
    public void StartMoveCam()
    {
        if (combatM.CheckMoveState() || combatM.CheckAttackState() || combatM.CheckAbilityState() || isCameraMoving || turnM.GetCurrentTurn().CompareTag("Enemy"))
            return;
        mousePos = Input.mousePosition;
        GameObject.Find("EventSystem").GetComponent<InputSystemUIInputModule>().enabled = false;
        combatUI.CameraUI(false);
        isCameraMoving = true;

    }
    public void MoveCam()
    {
        if (combatM.CheckMoveState() || combatM.CheckAttackState() || combatM.CheckAbilityState() || !isCameraMoving || turnM.GetCurrentTurn().CompareTag("Enemy"))
            return;
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        Vector3 moveDirection = new Vector3(mouseDelta.y, 0f, -mouseDelta.x).normalized;
        combatCam.transform.Translate(moveDirection * rotSpeed, Space.World);
        Debug.Log("Moving combat cam!!");
    }
    public void ResetCam()
    {
        if (combatM.CheckMoveState() || combatM.CheckAttackState() || combatM.CheckAbilityState() || !isCameraMoving || turnM.GetCurrentTurn().CompareTag("Enemy"))
            return;
        StopAllCoroutines();
        StartCoroutine(TransitionCamera(cameraPos, cameraRot));
    }
    IEnumerator TransitionCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = combatCam.transform.position;
        Quaternion startingRotation = combatCam.transform.rotation;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            combatCam.transform.position = Vector3.Lerp(startingPosition, targetPosition, t);
            combatCam.transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, t);

            yield return null;
        }

        combatCam.transform.position = targetPosition;
        combatCam.transform.rotation = targetRotation;

        GameObject.Find("EventSystem").GetComponent<InputSystemUIInputModule>().enabled = true;
        Mouse.current.WarpCursorPosition(mousePos);
        isCameraMoving = false;
        combatUI.CameraUI(true);
        Debug.Log("Camera Reset!");
    }
    public bool GetCameraMoving()
    {
        return isCameraMoving;
    }

}

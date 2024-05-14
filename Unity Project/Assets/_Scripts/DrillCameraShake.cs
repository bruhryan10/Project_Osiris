using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillCameraShake : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private float verticalRotation = 0.0f;
    private float horizontalRotation = 0.0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Camera rotation from mouse controls
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        horizontalRotation += mouseX * mouseSensitivity * Time.deltaTime;
        verticalRotation += mouseY * mouseSensitivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0.0f);

        

    }

}

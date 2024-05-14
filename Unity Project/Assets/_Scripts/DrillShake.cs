using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillShake : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 originalPosition;
    private float shakeTimer = 0f;
    private float shakeMagnitude = 0.1f;
    private float shakeDuration = 15f;

    private void Start()
    {
        cameraTransform = Camera.main.transform; // Assuming the camera is tagged as "MainCamera"
        originalPosition = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            // Shake the camera
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            // Reduce shake duration over time
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // Reset the camera position
            shakeTimer = 0f;
            cameraTransform.localPosition = originalPosition;
        }
    }

    public void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimer = duration;
    }

    public void StopShake()
    {
        shakeTimer = 0f;
    }
}

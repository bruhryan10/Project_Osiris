using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 15f; // Duration of the shake in seconds
    public float shakeMagnitude = 0.1f; // Intensity of the shake

    private DrillShake drillShake;

    private void OnEnable()
    {
        StartShake();
    }

    private void OnDisable()
    {
        StopShake();
    }

    private void StartShake()
    {
        drillShake = Camera.main.GetComponent<DrillShake>();
        if (drillShake != null)
        {
            drillShake.StartShake(shakeDuration, shakeMagnitude);
        }
        else
        {
            Debug.LogError("DrillShake script not found on main camera!");
        }
    }

    private void StopShake()
    {
        if (drillShake != null)
        {
            drillShake.StopShake();
        }
    }
}




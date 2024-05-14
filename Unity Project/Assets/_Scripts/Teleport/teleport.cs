using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public Transform destination; 
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        player.transform.position = destination.position; 
    }

}

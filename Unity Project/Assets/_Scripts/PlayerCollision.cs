using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] CombatManager combatM;
    [SerializeField] DungeonInputManager InteractM;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("COMBAT"))
        {
            GameObject currentEnemy = other.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            string enemyName = currentEnemy.name;
            combatM.SetCurrentEnemy(currentEnemy);
            StartCoroutine(combatM.StartCombatAnim(enemyName));
        }
        if (other.CompareTag("OBELISK"))
        {
            InteractM.SetInteractName("Obelisk");
            InteractM.SetInteractState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OBELISK"))
        {
            //Debug.Log("this");
            InteractM.SetInteractState(false);
            InteractM.SetInteractName("Bruh");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataFetcher : MonoBehaviour
{
    public CombatManager combatManager;

    public bool InCombat;
    public string PlayerStatus;
    // Start is called before the first frame update
    void Start()
    {
        InCombat = combatManager.CheckCombatState();

        if (InCombat) PlayerStatus = "In Combat";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

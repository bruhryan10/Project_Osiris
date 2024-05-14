using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [SerializeField] Canvas deathUI;
    [SerializeField] Canvas combatUI;
    [SerializeField] TurnManager turnM;
    [SerializeField] CombatManager combatM;
    [SerializeField] CombatRooms rooms;
    [SerializeField] GameObject palyer;
    [SerializeField] PartyManager party;
    PlayerStats stats;
    TileMovement tiles;
    bool isDead;

    private void Start()
    {
        deathUI.enabled = false;

    }
    void Update()
    {
        if (combatM.CheckCombatState())
        {
            int turns = 10 - combatM.GetTurnCount();
            GameObject currentObj = turnM.GetCurrentTurn();
            SetRef();
            if (currentObj.CompareTag("Enemy"))
                return;
            if (currentObj.CompareTag("Player") && stats.GetStat("currentHealth") <= 0 || combatM.GetObjective() == "FINAL_BOSS" && turns <= 0)
                PlayerDeath();
            if (currentObj.CompareTag("Ally") && stats.GetStat("currentHealth") <= 0)
                AllyDeath();
        }

    }
    public void SetRef()
    {
        GameObject currentObj = turnM.GetCurrentTurn();
        tiles = currentObj.gameObject.GetComponent<TileMovement>();
        stats = currentObj.gameObject.GetComponent<PlayerStats>();
    }
    public void AllyDeath()
    {
        GameObject currentObj = turnM.GetCurrentTurn();
        turnM.RemoveEnemy(currentObj);
        party.AllyDeath(currentObj);
        Debug.Log("ALLY DEATH!");
        rooms.activeArray[tiles.CheckMoveY(), tiles.CheckMoveX()] = "#";
        currentObj.SetActive(false);
        currentObj.transform.position = new Vector3(0, 0, 0);
    }
    public void PlayerDeath()
    {
        isDead = true;
        //Debug.Log("PLAYER DEATH!");
        combatUI.enabled = false;
        deathUI.enabled = true;
    }
    public void LoadSave()
    {
        party.ResetList();
        combatM.RestartCombat();
        isDead = false;
        deathUI.enabled = false;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Main Menu!");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
    public bool GetDeath()
    {
        return isDead;
    }
}

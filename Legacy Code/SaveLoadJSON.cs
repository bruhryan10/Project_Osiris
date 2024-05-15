using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerData
{
    public float speed;
    public float range;
    public float damage;
    public float health;
    public float luck;
    public Vector3 PlayerLocation;
    public Quaternion PlayerRotation;
    //public string[] sceneName = { "b" };
}
public class SaveLoadJSON : MonoBehaviour
{
    public PlayerData playerData = new PlayerData();
    public PlayerStats playerStats;
    [SerializeField] SafeRoomManager safeRoomM;
    [SerializeField] CombatManager combatM;

    string saveFilePath;
    // Start is called before the first frame update
    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/PlayerData.json";
    }
    public void FetchStats()
    {
        //playerData.sceneName[0] = SceneManager.GetActiveScene().name;
        playerData.speed = playerStats.GetStat("speed");
        playerData.range = playerStats.GetStat("range");
        playerData.damage = playerStats.GetStat("damage");
        playerData.health = playerStats.GetStat("health");
        playerData.luck = playerStats.GetStat("luck");
        playerData.PlayerLocation = playerStats.GetPlayerLocation();
        playerData.PlayerRotation = playerStats.GetPlayerRotation();
    }

    public void ApplyStats()
    {
        //SceneManager.LoadScene(playerData.sceneName[0]);
        playerStats.SetStat("speed", playerData.speed);
        playerStats.SetStat("range", playerData.range);
        playerStats.SetStat("damage", playerData.damage);
        playerStats.SetStat("health", playerData.health);
        playerStats.SetStat("luck", playerData.luck);
        playerStats.SetPlayerLocation(playerData.PlayerLocation);
        playerStats.SetPlayerRotation(playerData.PlayerRotation);

    }
    public void SaveGame()
    {
        Debug.Log("Saving game...");
        FetchStats();
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);
        safeRoomM.AfterSave();
        Debug.Log("Game Saved!");
    }
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            Debug.Log("Loading save...");
            string loadPlayerData = File.ReadAllText(saveFilePath);
            playerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
            if (combatM.CheckCombatState())
                safeRoomM.StartSafeRoom();
            combatM.AfterCombat();
            //LOAD AREA HERE
            ApplyStats();
            Debug.Log("Save Loaded!");
        }
        else
        {
            safeRoomM.NoLoad();
            Debug.Log("No save Detected!");
        }
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save Deleted!");
            safeRoomM.AfterDelete();
        }
        else
        {
            safeRoomM.NoDelete();
            Debug.Log("No save Detected!");
        }
    }
}

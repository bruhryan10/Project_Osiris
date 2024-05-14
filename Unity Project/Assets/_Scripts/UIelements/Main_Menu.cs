using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    
    public GameObject mainmenu;
    
    public void LoadGame()
    {
        SceneManager.LoadScene("IntroCutscene");
    }

    public void LoadSettings()
    {
        //SceneManager.LoadScene("Settings");
    }


    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quitgame()
    {
        Application.Quit();
    }
}

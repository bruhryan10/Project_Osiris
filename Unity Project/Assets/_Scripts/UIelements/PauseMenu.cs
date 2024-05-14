using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool paused = false;

    [SerializeField] Canvas pauseUI;
    [SerializeField] SafeRoomManager safeRoomM;
    [SerializeField] VerManager version;

    private void Awake()
    {
        Resume();
    }
    public void CheckPause()
    {
        if (paused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        pauseUI.enabled = false;
        Time.timeScale = 1;
        paused = false;
    }

    void Pause()
    {
        pauseUI.enabled = true;
        Time.timeScale = 0f;
        paused = true;

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quitgame()
    {
        Application.Quit();
    }
}

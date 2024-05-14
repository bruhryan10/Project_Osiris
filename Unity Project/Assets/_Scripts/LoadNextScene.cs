using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextSceneAsync();
        }
    }

    void LoadNextSceneAsync()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        SceneManager.LoadSceneAsync(nextSceneIndex);
    }
}

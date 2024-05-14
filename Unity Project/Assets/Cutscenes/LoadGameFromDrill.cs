using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadGameFromDrill : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("Prototype", LoadSceneMode.Single);
    }

}

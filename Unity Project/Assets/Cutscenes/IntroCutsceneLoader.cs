using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutsceneLoader : MonoBehaviour
{


    private void OnEnable()
    {
        SceneManager.LoadScene("DrillCutscene", LoadSceneMode.Single);
    }
}

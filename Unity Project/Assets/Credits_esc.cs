using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Credits_esc : MonoBehaviour
{
    void Update()
    {

        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
            

}

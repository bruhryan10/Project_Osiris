using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    private int MusicStuff;
    private int AmountOfScenes = 2; 
    
    private void Awake()
    {

        MusicStuff = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        
        int someMusicStuff = MusicStuff + AmountOfScenes;

        
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex >= someMusicStuff)
        {
            
            Destroy(gameObject);
        }
    }
}

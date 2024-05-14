using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Audio : MonoBehaviour
{
    public AudioSource myFx;
    public AudioClip hovorFx;
    public AudioClip ClickFx;

    public void HoverSound()
    {
        myFx.PlayOneShot(hovorFx);
    }
    public void ClickSound()
    {
        myFx.PlayOneShot(ClickFx);
    }
}

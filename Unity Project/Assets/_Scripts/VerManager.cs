using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerManager : MonoBehaviour
{
    [SerializeField] string[] ver = new string[] { "A0.03" };
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public string GetVersion()
    {
        return ver[0];
    }
    public void VersionChanges(string num)
    {
        if (num == "A0.01")
            A001();
        if (num == "A0.02")
            A002();
        if (num == "A0.02.1")
            A0021();
        if (num == "A0.03")
            A003();
    }
    void A001()
    {
        Debug.Log("A0.01 Changes: \nAdded this thing called a version log");
        Debug.Log(" - Added pause menu to main scene");
        Debug.Log(" - Added Both Support & Tank Passive Abilities");
        Debug.Log(" --Added Visual to show how big the passive area is (T)");
    }
    void A002()
    {
        Debug.Log("A0.02 Changes:");
        Debug.Log("Small Battle screen transitions added");
        Debug.Log("Tank Shield Ability Added");
    }
    void A0021()
    {
        Debug.Log("A0.02.1 Changes:");
        Debug.Log("Fixed massive bug where you could not fight more than one enemy during an instance (lol)");
    }
    void A003()
    {
        Debug.Log("A0.03 Changes:");
        Debug.Log("Support Heal Ability Added");
    }


}

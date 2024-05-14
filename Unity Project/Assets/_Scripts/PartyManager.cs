using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    [SerializeField] List<GameObject> partyMembers;
    [SerializeField] List<GameObject> activeMembers;
    [SerializeField] List<GameObject> inactiveMembers;

    private void Start()
    {
        foreach (GameObject obj in partyMembers)
        {
            stats = obj.GetComponent<PlayerStats>();
            if (stats.GetStat("currentHealth") <= 0)
            {
                inactiveMembers.Add(obj);
                activeMembers.Remove(obj);
            }
            else
                activeMembers.Add(obj);
        }

    }
    public List<GameObject> GetActiveMembers()
    {
        return activeMembers;
    }
    public void AllyDeath(GameObject obj)
    {
        inactiveMembers.Add(obj);
        activeMembers.Remove(obj);
    }
    public void ResetList()
    {
        inactiveMembers.Clear();
        activeMembers.Clear();
        activeMembers = new List<GameObject>(partyMembers);
    }
}

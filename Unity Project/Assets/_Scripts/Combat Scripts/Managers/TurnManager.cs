using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<GameObject> turnOrder = new List<GameObject>();
    public void AddPlayer(GameObject player)
    {
        turnOrder.Add(player);
    }
    public void RemovePlayer(GameObject player)
    {
        turnOrder.Remove(player);
    }
    public void AddEnemy(GameObject enemy)
    {
        turnOrder.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        turnOrder.Remove(enemy);
    }
    public void AddAlly(GameObject ally) 
    {
        int index = turnOrder.FindIndex(obj => obj.CompareTag("Enemy"));

        if (index == -1)
        {
            turnOrder.Add(ally);
        }
        else
            turnOrder.Insert(index, ally);
    }
    public void ChangeTurnOrder()
    {
        GameObject activeObject = turnOrder[0];
        turnOrder.RemoveAt(0);
        turnOrder.Add(activeObject);
    }
    public GameObject GetCurrentTurn()
    {
        return turnOrder[0];
    }
    public List<GameObject> TurnMembers()
    {
        return turnOrder;
    }
    public void clearTurnOrder()
    {
        for (int i = turnOrder.Count - 1; i >= 0; i--)
        {
            GameObject obj = turnOrder[i];
            turnOrder.RemoveAt(i);
        }
    }
}

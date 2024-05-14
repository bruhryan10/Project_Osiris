using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class TileMovement : MonoBehaviour
{

    [Header("Script References")]
    [SerializeField] CombatManager combatM;
    [SerializeField] CombatActions combatA;
    [SerializeField] CombatRooms rooms;
    [SerializeField] PlayerStats stats;
    [SerializeField] TurnManager turnManager;

    [SerializeField] int x = 3;
    [SerializeField] int y = 22;
    int moveNum = 2;
    float currentMoves = 0;
    Vector3 lastPosNum;
    public string allyNum;
    [SerializeField] float duration;
    Quaternion playerRot;
    void Start()
    {
        rooms = GameObject.Find("CombatManager").GetComponent<CombatRooms>();
        combatM = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        turnManager = GameObject.Find("CombatManager").GetComponent<TurnManager>();
    }

    public void Controls(string input)
    {
        if (currentMoves >= stats.GetStat("speed"))
            return;
        if (input == "left")
        {
            StartRot(0);
            Debug.Log(rooms.activeArray[17 - y, x - 1]);
            if (x - 1 < 0 || rooms.activeArray[17 - y, x - 1] == "|" || rooms.activeArray[17 - y, x - 1] == "e")
                return;
            string move = rooms.activeArray[17 - y, x - 1];
            if (this.CompareTag("Player") && move[0] == 'a')
                return;
            if (this.CompareTag("Ally") && (move == "p" || (move[0] == 'a' && move[1] != this.allyNum[1])))
                return;
                x -= 1;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveNum);
            currentMoves++;
        }
        if (input == "right")
        {
            StartRot(180);
            if (x + 1 >= rooms.activeArray.Length / rooms.activeArray.GetLength(0) || rooms.activeArray[17 - y, x + 1] == "|" || rooms.activeArray[17 - y, x + 1] == "e")
                return;
            string move = rooms.activeArray[17 - y, x + 1];
            if (this.CompareTag("Player") && move[0] == 'a')
                return;
            if (this.CompareTag("Ally") && (move == "p" || (move[0] == 'a' && move[1] != this.allyNum[1])))
                return;
            x += 1;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveNum);
            currentMoves++;
        }
        if (input == "down")
        {
            StartRot(-90);
            if (rooms.activeArray.GetLength(0) <= y + 1 || rooms.activeArray[17 - y + 1, x] == "|" || rooms.activeArray[17 - y + 1, x] == "e")
                return;
            string move = rooms.activeArray[17 - y + 1, x];
            if (this.CompareTag("Player") && move[0] == 'a')
                return;
            if (this.CompareTag("Ally") && (move == "p" || (move[0] == 'a' && move[1] != this.allyNum[1])))
                return;
            y -= 1;
            transform.position = new Vector3(transform.position.x - moveNum, transform.position.y, transform.position.z);
            currentMoves++;
        }
        if (input == "up")
        {
            StartRot(90);
            if (17 - y + 1 < 0 || rooms.activeArray[17 - y - 1, x] == "|" || rooms.activeArray[17 - y - 1, x] == "e")
                return;
            string move = rooms.activeArray[17 - y - 1, x];
            if (this.CompareTag("Player") && move[0] == 'a')
                return;
            if (this.CompareTag("Ally") && (move == "p" || (move[0] == 'a' && move[1] != this.allyNum[1])))
                return;
            y += 1;
            transform.position = new Vector3(transform.position.x + moveNum, transform.position.y, transform.position.z);
            currentMoves++;
        }
    }

    public void ResetMove()
    {
        if (currentMoves == 0)
            combatM.BackOption();
        StopAllCoroutines();
        StartCoroutine(ResetMoveAnim(combatM.CheckCurrentPos()));
    }

    IEnumerator ResetMoveAnim(Vector3 targetPosition)
    {
        Vector3 initialPosition = turnManager.GetCurrentTurn().transform.position;
        float elapsedTime = 0f;
        float duration = 0.1f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        turnManager.GetCurrentTurn().transform.position = targetPosition;
        currentMoves = 0;
        x = combatM.CheckCurrentPlayerX();
        y = combatM.CheckCurrentPlayerY();
        int bruh = Mathf.RoundToInt(playerRot.eulerAngles.y);
        StartRot(bruh);
    }
    public void AfterMoving()
    {
        currentMoves = 0;
        combatM.EndTurn();
        UpdateOldArray();
        UpdateArray();
    }
    public void UpdateArray()
    {
        if (this.CompareTag("Player"))
            rooms.activeArray[17 - y, x] = "p";
        if (this.CompareTag("Ally"))
            rooms.activeArray[17 - y, x] = this.allyNum;
    }
    void UpdateOldArray()
    {
        int lastY = Mathf.RoundToInt(lastPosNum.y);
        int lastX = Mathf.RoundToInt(lastPosNum.x);
        rooms.activeArray[17 - lastY, lastX] = "#";
    }
    public void GetArrayPos()
    {
        lastPosNum = new Vector3(x, y);
    }
    public float GetCurrentSpeed()
    {
        return stats.GetStat("speed") - currentMoves;
    }
    public int CheckMoveX()
    {
        return x;
    }
    public void SetMoveX(int newX)
    {
        x = newX;
    }
    public int CheckMoveY()
    {
        return y;
    }
    public void SetMoveY(int newY)
    {
        y = newY;
    }
    public int CheckMoveNum()
    {
        return moveNum;
    }
    public void SetAllyNum(string input)
    {
        allyNum = input;
    }
    public Vector3 GetTransform()
    {
        return new Vector3(164.24f + (2 * y), 16.67322f, -15.381f + (2 * x));
    }
    void StartRot(int rotNum)
    {
        StopAllCoroutines();
        StartCoroutine(RotateMesh(rotNum));
    }
    IEnumerator RotateMesh(int rotNum)
    {
        Transform GetMesh = this.gameObject.transform.Find("Mesh");
        GameObject currentMesh = GetMesh.gameObject;

        Quaternion startRotation = currentMesh.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, rotNum, 0);

        float elapsedTime = 0f;
        duration = 0.2f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            currentMesh.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        currentMesh.transform.rotation = endRotation;
    }
    public void GetRot()
    {
        Transform GetMesh = turnManager.GetCurrentTurn().transform.Find("Mesh");
        playerRot = GetMesh.rotation;
    }
    public Quaternion GetPlayerRot()
    {
        return playerRot;
    }
}

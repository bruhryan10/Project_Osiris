using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CombatAttack : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] CombatManager combatM;
    [SerializeField] CombatRooms rooms;
    [SerializeField] TurnManager turnManager;
    [SerializeField] CombatActions combatA;
    [SerializeField] PlayerStats playerStats;
    //---------------------------------------
    [SerializeField] TileMovement tiles;
    [SerializeField] PlayerStats stats;
    [SerializeField] AttackTileCollision attackTile;

    bool takeDamage;
    float currentRange = 0;
    int attackX;
    int attackY;
    Quaternion playerRot;
    void Update()
    {
        if (!combatM.CheckCombatState())
            return;
        GameObject currentObj = turnManager.GetCurrentTurn();
        if (currentObj.CompareTag("Player") || currentObj.CompareTag("Ally"))
        {
            SetRef();
            if (combatM.CheckAttackState())
                combatA.SetAttackAreaState(true);
        }
    }
    public void Controls(string input)
    {
        if (currentRange >= stats.GetStat("range"))
            return;
        GameObject attack = combatA.GetAttackArea();
        if (input == "left")
        {
            if (attackX - 1 < 0 || rooms.activeArray[17 - attackY, attackX - 1] == "|")
                return;
            attackX -= 1;
            combatA.SetAttackAreaPosition(new Vector3(attack.transform.position.x, attack.transform.position.y, attack.transform.position.z + 2));
            currentRange++;
            StartRot(false);
        }
        if (input == "right")
        {
            if (attackX + 1 >= rooms.activeArray.Length / rooms.activeArray.GetLength(0) || rooms.activeArray[17 - attackY, attackX + 1] == "|")
                return;
            attackX += 1;
            combatA.SetAttackAreaPosition(new Vector3(attack.transform.position.x, attack.transform.position.y, attack.transform.position.z - 2));
            currentRange++;
            StartRot(false);
        }
        if (input == "down")
        {
            if (rooms.activeArray.GetLength(0) <= attackY + 1 | rooms.activeArray[17 - attackY + 1, attackX] == "|")
                return;
            attackY -= 1;
            combatA.SetAttackAreaPosition(new Vector3(attack.transform.position.x - 2, attack.transform.position.y, attack.transform.position.z));
            currentRange++;
            StartRot(false);
        }
        if (input == "up")
        {
            if (17 - attackY + 1 < 0 || rooms.activeArray[17 - attackY - 1, attackX] == "|")
                return;
            attackY += 1;
            combatA.SetAttackAreaPosition(new Vector3(attack.transform.position.x + 2, attack.transform.position.y, attack.transform.position.z));
            currentRange++;
            StartRot(false);
        }
    }

    public void ResetAttack()
    {
        if (currentRange == 0)
            combatM.BackOption();
        StopAllCoroutines();
        StartCoroutine(ResetAttackAnim());
    }
    IEnumerator ResetAttackAnim()
    {
        Vector3 targetPosition = new Vector3(combatM.CheckCurrentPos().x, combatM.CheckCurrentPos().y - 0.98f, combatM.CheckCurrentPos().z);
        Vector3 startPosition = combatA.GetAttackAreaPosition();

        float elapsedTime = 0f;
        float duration = 0.1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            combatA.SetAttackAreaPosition(Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration));
            yield return null;
        }

        combatA.SetAttackAreaPosition(targetPosition);
        currentRange = 0;
        SetAttackX(tiles.CheckMoveX());
        SetAttackY(tiles.CheckMoveY());
        attackTile.SetTargetNull();
        StartRot(true);
    }
    public void AfterAttack()
    {
        currentRange = 0;
        combatA.SetAttackAreaPosition(new Vector3(combatM.CheckCurrentPos().x, combatM.CheckCurrentPos().y - 0.98f, combatM.CheckCurrentPos().z));
        combatM.SetMoveState(false);
        SetAttackX(combatM.CheckCurrentPlayerX());
        SetAttackY(combatM.CheckCurrentPlayerY());
        combatM.EndTurn();
    }
    public void GetRot()
    {
        Transform GetMesh = turnManager.GetCurrentTurn().transform.Find("Mesh");
        playerRot = GetMesh.rotation;
    }
    public void StartRot(bool reset)
    {
        StopAllCoroutines();
        if (attackX == tiles.CheckMoveX() && attackY == tiles.CheckMoveY())
            reset = true;
        if (reset)
            StartCoroutine(ResetMesh());
        else
            StartCoroutine(RotateMesh());
    }
    IEnumerator ResetMesh()
    {
        Transform getMesh = turnManager.GetCurrentTurn().transform.Find("Mesh");
        GameObject currentMesh = getMesh.gameObject;

        Quaternion startRotation = currentMesh.transform.rotation;
        Quaternion targetRotation = playerRot;

        float startTime = Time.time;
        float elapsedTime = 0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            currentMesh.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            elapsedTime = Time.time - startTime;
            yield return null;
        }
        currentMesh.transform.rotation = targetRotation;
    }
    IEnumerator RotateMesh()
    {
        Transform getMesh = turnManager.GetCurrentTurn().transform.Find("Mesh");
        Transform getTile = turnManager.GetCurrentTurn().gameObject.transform.Find("Attack Tile");
        GameObject currentMesh = getMesh.gameObject;

        Vector3 dir = getTile.position - currentMesh.transform.position;
        dir.y = 0;
        Quaternion startRotation = currentMesh.transform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(dir);

        float elapsedTime = 0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            currentMesh.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentMesh.transform.rotation = endRotation; // Ensure final rotation is exactly the target rotation
    }
    void LookAtTile()
    {
        Transform GetMesh = turnManager.GetCurrentTurn().transform.Find("Mesh");
        Transform GetTile = turnManager.GetCurrentTurn().gameObject.transform.Find("Attack Tile");
        GameObject currentMesh = GetMesh.gameObject;

        Vector3 dir = GetTile.position - currentMesh.transform.position;
        dir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(dir);
        currentMesh.transform.rotation = rotation;
    }
    public void WithinAttack()
    {
         attackTile.TargetDamage(stats.GetStat("damage"));
    }
    public float GetCurrentRange()
    {
        return stats.GetStat("range") - currentRange;
    }
    public int CheckAttackX()
    {
        return attackX;
    }
    public void SetAttackX(int newX)
    {
        attackX = newX;
    }
    public void AddAttackX(int input)
    {
        attackX += input;
    }
    public void AddAttackY(int input)
    {
        attackY += input;
    }
    public int CheckAttackY()
    {
        return attackY;
    }
    public void SetAttackY(int newY)
    {
        attackY = newY;
    }
    public void SetRef()
    {
        GameObject currentObj = turnManager.GetCurrentTurn();
        tiles = currentObj.gameObject.GetComponent<TileMovement>();
        stats = currentObj.gameObject.GetComponent<PlayerStats>();
        attackTile = currentObj.transform.Find("Attack Tile").GetComponent<AttackTileCollision>();
        playerStats = currentObj.GetComponent<PlayerStats>();
    }
}

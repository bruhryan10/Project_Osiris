using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] AbilityManager abilityM;
    [SerializeField] GameObject[] attackOneRange;
    [SerializeField] CombatRooms rooms;
    [SerializeField] AttackTileCollision attackTile;
    [SerializeField] TileMovement playerTiles;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] CombatManager combatM;
    [SerializeField] CombatActions combatA;
    [SerializeField] CombatAttack combatAttack;
    [SerializeField] TurnManager turnManager;
    [SerializeField] NewCombatUI UI;
    [SerializeField] ObjectiveManager objectiveM;
    [SerializeField] int overdriveCooldown;
    [SerializeField] bool overdriveActive;
    [SerializeField] int all;
    [SerializeField] GameObject airstrikeAnim;
    [SerializeField] GameObject attackOneAnim;
    int tempX;
    int tempY;

    public Animator animator;

    public void SummonAbility()
    {
        Debug.Log("summon");
    }
    public void SiphonAbility()
    {
        Debug.Log("siphon");
    }
    public void ConfirmOverdrive()
    {
        if (playerStats.GetStat("currentHealth") <= 4)
            return;
        animator.SetTrigger("isOverdrive");
        Debug.Log("Overdrive triggered.");
        overdriveCooldown = 4;
        overdriveActive = true;
        playerStats.IncreaseStat("currentHealth", -4);
        playerStats.IncreaseStat("range", 3);
        playerStats.IncreaseStat("damage", 3);
        Debug.Log("overdrive");
        UI.ShowDamage(Player, 4, false);
        ResetOverdrive();
        combatM.AddAbilityCount();
        combatM.EndTurn();
    }
    public void ResetOverdrive()
    {
        abilityM.SetActiveAbility("bruh");
    }
    public void ConfirmAttackAbilityOne()
    {
        for (int i = 0; i < attackOneRange.Length; i++)
        {
            animator.SetTrigger("isShoot");
            attackTile = attackOneRange[i].GetComponent<AttackTileCollision>();
            if (!attackTile.IsTargetNull())
            {
                all += 1;
                combatM.AddAbilityCount();
                attackOneAnim.SetActive(false);
                attackOneAnim.SetActive(true);
            }
            attackTile.TargetDamage(5);
        }
        if (all == 0)
            Debug.Log("Zero enemies");
        if (all > 1 && objectiveM.GetSecondaryNum() == 3)
            objectiveM.SetSubObjective();

    }
    public void ResetAttackAbilityOne()
    {
        abilityM.SetActiveAbility("bruh");
        for (int i = 0; i < attackOneRange.Length; i++)
            attackOneRange[i].SetActive(false);
        int bruh = Mathf.RoundToInt(playerTiles.GetPlayerRot().eulerAngles.y);
        StartRot(bruh);
    }
    public void StartAirstrike()
    {
        if (playerStats.GetStat("currentHealth") <= 2)
            return;
        combatA.SetAttackAreaState(true);
        combatM.SetCurrentPos(new Vector3(turnManager.GetCurrentTurn().transform.position.x, turnManager.GetCurrentTurn().transform.position.y, turnManager.GetCurrentTurn().transform.position.z));
        combatAttack.SetAttackX(playerTiles.CheckMoveX());
        combatAttack.SetAttackY(playerTiles.CheckMoveY());
        tempX = playerTiles.CheckMoveX();
        tempY = playerTiles.CheckMoveY();
    }
    public void AirstrikeBack()
    {
        if (tempX == combatAttack.CheckAttackX() && tempY == combatAttack.CheckAttackY())
        {
            abilityM.SetActiveAbility("bruh");
            combatA.SetAttackArea(false);
            UI.AbilityUI();
        }
        combatA.SetAttackAreaPosition(new Vector3(combatM.CheckCurrentPos().x, combatM.CheckCurrentPos().y - 0.98f, combatM.CheckCurrentPos().z));
        combatAttack.SetAttackX(playerTiles.CheckMoveX());
        combatAttack.SetAttackY(playerTiles.CheckMoveY());
        tempX = playerTiles.CheckMoveX();
        tempY = playerTiles.CheckMoveY();
    }
    public void AirstrikeConfirm()
    {
        attackTile = combatA.GetAttackArea().GetComponent<AttackTileCollision>();
        if (attackTile.IsTargetNull())
            return;

        animator.SetTrigger("isAirstrike");
        AirstrikeAnimStart();
        Debug.Log("Airstrike triggered.");
    }
    public void AttackOneAnimStart()
    {
        attackOneAnim.SetActive(false);
        attackOneAnim.SetActive(true);
        Debug.Log(attackOneAnim.activeInHierarchy);
        attackTile.TargetDamage(3);
        combatA.SetAttackAreaPosition(new Vector3(combatM.CheckCurrentPos().x, combatM.CheckCurrentPos().y - 0.98f, combatM.CheckCurrentPos().z));
        combatAttack.SetAttackX(playerTiles.CheckMoveX());
        combatAttack.SetAttackY(playerTiles.CheckMoveY());
        combatM.EndTurn();
    }
    public void AirstrikeAnimStart()
    {
        StopAllCoroutines();
        StartCoroutine(AirstrikeSendAnimBruh());
    }
    IEnumerator AirstrikeSendAnimBruh()
    {
        GameObject anim = Instantiate(airstrikeAnim);
        anim.transform.position = new Vector3(combatA.GetAttackArea().transform.position.x, combatA.GetAttackArea().transform.position.y + 15f, combatA.GetAttackArea().transform.position.z);
        //Vector3 initialPosition = anim.transform.position;
        //Vector3 targetPosition = new Vector3(combatA.GetAttackArea().transform.position.x, combatA.GetAttackArea().transform.position.y, combatA.GetAttackArea().transform.position.z);
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            //anim.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

       // anim.transform.position = targetPosition;
        attackTile.TargetDamage(3);
        combatA.SetAttackAreaPosition(new Vector3(combatM.CheckCurrentPos().x, combatM.CheckCurrentPos().y - 0.98f, combatM.CheckCurrentPos().z));
        combatAttack.SetAttackX(playerTiles.CheckMoveX());
        combatAttack.SetAttackY(playerTiles.CheckMoveY());
        //Destroy(anim);
        combatM.EndTurn();
    }
    public void AirstrikeMove(string input)
    {
        GameObject attack = combatA.GetAttackArea();
        if (input == "left")
        {
            if (combatAttack.CheckAttackX() - 1 < 0 || rooms.activeArray[17 - combatAttack.CheckAttackY(), combatAttack.CheckAttackX() - 1] == "|")
                return;
            combatAttack.AddAttackX(-1);
            combatA.SetAttackAreaPosition(new Vector3(attack.transform.position.x, attack.transform.position.y, attack.transform.position.z + 2));
        }
        if (input == "right")
        {
            if (combatAttack.CheckAttackX() + 1 >= rooms.activeArray.Length / rooms.activeArray.GetLength(0) || rooms.activeArray[17 - combatAttack.CheckAttackY(), combatAttack.CheckAttackX() + 1] == "|")
                return;
            combatAttack.AddAttackX(1);
            combatA.SetAttackAreaPosition(new Vector3(attack.transform.position.x, attack.transform.position.y, attack.transform.position.z - 2));
        }
        if (input == "down")
        {
            if (rooms.activeArray.GetLength(0) <= combatAttack.CheckAttackY() + 1 | rooms.activeArray[17 - combatAttack.CheckAttackY() + 1, combatAttack.CheckAttackX()] == "|")
                return;
            combatAttack.AddAttackY(-1);
            combatA.SetAttackAreaPosition(new Vector3(attack.transform.position.x - 2, attack.transform.position.y, attack.transform.position.z));
        }
        if (input == "up")
        {
            if (17 - combatAttack.CheckAttackY() + 1 < 0 || rooms.activeArray[17 - combatAttack.CheckAttackY() - 1, combatAttack.CheckAttackX()] == "|")
                return;
            combatAttack.AddAttackY(1);
            combatA.SetAttackAreaPosition(new Vector3(attack.transform.position.x + 2, attack.transform.position.y, attack.transform.position.z));
        }
    }
    void StartRot(int rotNum)
    {
        StopAllCoroutines();
        StartCoroutine(RotateMesh(rotNum));
    }
    IEnumerator RotateMesh(int rotNum)
    {
        Transform GetMesh = turnManager.GetCurrentTurn().gameObject.transform.Find("Mesh");
        GameObject currentMesh = GetMesh.gameObject;

        Quaternion startRotation = currentMesh.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, rotNum, 0);

        float elapsedTime = 0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            currentMesh.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        currentMesh.transform.rotation = endRotation;
    }
    public void AttackAbilityOne(string input)
    {
        if (playerStats.GetStat("currentHealth") <= 2)
            return;
        Debug.Log("Attack One");
        for (int i = 0; i < attackOneRange.Length; i++)
            attackOneRange[i].SetActive(false);

        for (int i = 0; i < attackOneRange.Length; i++)
        {
            GameObject obj = attackOneRange[i];
            float offset = i * 2;
            int playerX = playerTiles.CheckMoveX();
            int playerY = playerTiles.CheckMoveY();

            if (input == "left")
                playerX -= (i + 1);
            else if (input == "right")
                playerX += (i + 1);
            else if (input == "up")
                playerY += (i + 1);
            else if (input == "down")
                playerY -= (i + 1);
            if (rooms.CheckWall(playerY, playerX))
                break;
            else
            {
                if (input == "left")
                {
                    obj.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - 0.9899999f, Player.transform.position.z + 2 + offset);
                    StartRot(0);
                }
                else if (input == "right")
                {
                    obj.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - 0.9899999f, Player.transform.position.z - 2 - offset);
                    StartRot(180);
                }
                else if (input == "up")
                {
                    obj.transform.position = new Vector3(Player.transform.position.x + 2 + offset, Player.transform.position.y - 0.9899999f, Player.transform.position.z);
                    StartRot(90);
                }
                else if (input == "down")
                {
                    obj.transform.position = new Vector3(Player.transform.position.x - 2 - offset, Player.transform.position.y - 0.9899999f, Player.transform.position.z);
                    StartRot(-90);
                }
                obj.SetActive(true);

            }

        }
    }
    public bool GetOverdriveState()
    {
        return overdriveActive;
    }
    public void SetOverdriveState(bool input)
    {
        overdriveActive = input;
    }
    public int GetOverdriveCooldown()
    {
        return overdriveCooldown;
    }
    public void SetOverdriveCooldown(int input)
    {
        overdriveCooldown += input;
    }




}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class EnemyRays : MonoBehaviour
{
    EnemyAttack enemyAttack;
    EnemyMovement enemyMovement;
    EnemyStats enemyStats;

    public List<GameObject> enemyHit;
    public bool sightedPartyMember;

    Ray ray_forward;
    Ray ray_back;
    Ray ray_left;
    Ray ray_right;
    Ray ray_leftforward;
    Ray ray_leftback;
    Ray ray_rightforward;
    Ray ray_rightback;
    RaycastHit hitForward;
    RaycastHit hitBack;
    RaycastHit hitLeft;
    RaycastHit hitRight;
    RaycastHit hitRightForward;
    RaycastHit hitRightBack;
    RaycastHit hitLeftForward;
    RaycastHit hitLeftBack;

    // Start is called before the first frame update
    void Start()
    {
        ray_forward = new(transform.position, transform.forward);
        ray_back = new(transform.position, transform.forward * -1);
        ray_right = new(transform.position, transform.right);
        ray_left = new(transform.position, transform.right * -1);
        ray_leftforward = new(transform.position, transform.forward + transform.right);
        ray_leftback = new(transform.position, transform.forward - transform.right);
        ray_rightforward = new(transform.position, transform.forward * -1 + transform.right);
        ray_rightback = new(transform.position, transform.forward * -1 - transform.right);
        enemyMovement = this.GetComponent<EnemyMovement>();
        enemyAttack = this.GetComponent<EnemyAttack>();
        enemyStats = this.GetComponent<EnemyStats>();
        sightedPartyMember = false;
    }

    // Update is called once per frame
    void Update()
    {
        ray_forward.origin = transform.position;
        ray_back.origin = transform.position;
        ray_right.origin = transform.position;
        ray_left.origin = transform.position;
        ray_forward.direction = transform.forward;
        ray_back.direction = transform.forward * -1;
        ray_right.direction = transform.right;
        ray_left.direction = transform.right * -1;
        ray_leftforward.origin = transform.position;
        ray_leftforward.direction = transform.forward + transform.right;
        ray_leftback.origin = transform.position;
        ray_leftback.direction = transform.forward - transform.right;
        ray_rightforward.origin = transform.position;
        ray_rightforward.direction = transform.forward * -1 + transform.right;
        ray_rightback.origin = transform.position;
        ray_rightback.direction = transform.forward * -1 - transform.right;
        Debug.DrawRay(ray_forward.origin, ray_forward.direction * 8, Color.green);
        Debug.DrawRay(ray_back.origin, ray_back.direction * 8, Color.red);
        Debug.DrawRay(ray_right.origin, ray_right.direction * 8, Color.blue);
        Debug.DrawRay(ray_left.origin, ray_left.direction * 8, Color.yellow);
        Debug.DrawRay(ray_leftforward.origin, ray_leftforward.direction * 8, Color.green);
        Debug.DrawRay(ray_leftback.origin, ray_leftback.direction * 8, Color.red);
        Debug.DrawRay(ray_rightforward.origin, ray_rightforward.direction * 8, Color.blue);
        Debug.DrawRay(ray_rightback.origin, ray_rightback.direction * 8, Color.yellow);

        if (Regex.IsMatch(gameObject.name, @"^Repair\sEnemy$"))
            RepairEnemy();
        else if (Regex.IsMatch(gameObject.name, @"^Enemy$"))
            AttackerEnemy();
        else if (Regex.IsMatch(gameObject.name, @"^Defender\sEnemy$"))
            DefenderEnemy();
        else if (Regex.IsMatch(gameObject.name, @"^Boss\sEnemy$"))
            BossEnemy();
    }
    void AttackerEnemy()
    {
        if (Physics.Raycast(ray_forward, out hitForward, 2.9f))
        {
            if (hitForward.collider.gameObject.CompareTag("Player") || hitForward.collider.gameObject.name == "Support Ally" || hitForward.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockUp = false;
                if (!enemyHit.Contains(hitForward.collider.gameObject))
                    enemyHit.Add(hitForward.collider.gameObject);
            }

            if (hitForward.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockUp = true;
            }
        }
        else
        {
            enemyMovement.blockUp = false;
        }
        if (Physics.Raycast(ray_back, out hitBack, 2.9f))
        {
            if (hitBack.collider.gameObject.CompareTag("Player") || hitBack.collider.gameObject.name == "Support Ally" || hitBack.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockDown = false;
                if (!enemyHit.Contains(hitBack.collider.gameObject))
                    enemyHit.Add(hitBack.collider.gameObject);
            }
            if (hitBack.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockDown = true;
            }
        }
        else
        {
            enemyMovement.blockDown = false;
        }
        if (Physics.Raycast(ray_right, out hitRight, 2.9f))
        {
            if (hitRight.collider.gameObject.CompareTag("Player") || hitRight.collider.gameObject.name == "Support Ally" || hitRight.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockRight = false;
                if (!enemyHit.Contains(hitRight.collider.gameObject))
                    enemyHit.Add(hitRight.collider.gameObject);
            }
            if (hitRight.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockRight = true;
            }
        }
        else
        {
            enemyMovement.blockRight = false;
            GameObject hitObject = hitRight.collider ? hitRight.collider.gameObject : null;
            if (hitObject != null && enemyHit.Contains(hitObject))
                enemyHit.Remove(hitObject);
        }
        if (Physics.Raycast(ray_left, out hitLeft, 2.9f))
        {
            if (hitLeft.collider.gameObject.CompareTag("Player") || hitLeft.collider.gameObject.name == "Support Ally" || hitLeft.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockLeft = false;
                if (!enemyHit.Contains(hitLeft.collider.gameObject))
                    enemyHit.Add(hitLeft.collider.gameObject);
            }
            if (hitLeft.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockLeft = true;
            }
        }
        else
        {
            enemyMovement.blockLeft = false;
        }
        if (Physics.Raycast(ray_leftforward, out hitLeftForward, 2.9f))
        {
            if (hitLeftForward.collider.gameObject.CompareTag("Player") || hitLeftForward.collider.gameObject.name == "Support Ally" || hitLeftForward.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitLeftForward.collider.gameObject))
                    enemyHit.Add(hitLeftForward.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_leftback, out hitLeftBack, 2.9f))
        {
            if (hitLeftBack.collider.gameObject.CompareTag("Player") || hitLeftBack.collider.gameObject.name == "Support Ally" || hitLeftBack.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitLeftBack.collider.gameObject))
                    enemyHit.Add(hitLeftBack.collider.gameObject);
            }
            if (hitLeftBack.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockLeft = true;
            }
        }
        else
        {
            enemyMovement.blockLeft = false;
        }
        if (Physics.Raycast(ray_rightforward, out hitRightForward, 2.9f))
        {
            if (hitRightForward.collider.gameObject.CompareTag("Player") || hitRightForward.collider.gameObject.name == "Support Ally" || hitRightForward.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitRightForward.collider.gameObject))
                    enemyHit.Add(hitRightForward.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_rightback, out hitRightBack, 2.9f))
        {
            if (hitRightBack.collider.gameObject.CompareTag("Player") || hitRightBack.collider.gameObject.name == "Support Ally" || hitRightBack.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitRightBack.collider.gameObject))
                    enemyHit.Add(hitRightBack.collider.gameObject);
            }
        }
        if (enemyHit.Count > 0)
        {
            for (int i = enemyHit.Count - 1; i >= 0; i--)
            {
                if (!Physics.Raycast(ray_forward, out hitForward, 2.9f) && !Physics.Raycast(ray_back, out hitBack, 2.9f) && !Physics.Raycast(ray_left, out hitLeft, 2.9f) && !Physics.Raycast(ray_right, out hitRight, 2.9f) && !Physics.Raycast(ray_leftforward, out hitLeftForward, 2.9f) && !Physics.Raycast(ray_leftback, out hitLeftBack, 2.9f) && !Physics.Raycast(ray_rightforward, out hitRightForward, 2.9f) && !Physics.Raycast(ray_rightback, out hitRightBack, 2.9f))
                {
                    enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_forward, out hitForward, 2.9f))
                {
                    if (hitForward.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_back, out hitBack, 2.9f))
                {
                    if (hitBack.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_right, out hitRight, 2.9f))
                {
                    if (hitRight.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_left, out hitLeft, 2.9f))
                {
                    if (hitLeft.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_leftforward, out hitLeftForward, 2.9f))
                {
                    if (hitLeftForward.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_leftback, out hitLeftBack, 2.9f))
                {
                    if (hitLeftBack.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_rightforward, out hitRightForward, 2.9f))
                {
                    if (hitRightForward.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_rightback, out hitRightBack, 2.9f))
                {
                    if (hitRightBack.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (hitForward.collider.gameObject.name != enemyHit[i].name)
                {
                    // Delete object from list.
                    enemyHit.RemoveAt(i);
                }
            }
        }
    }
    void DefenderEnemy()
    {
        if (Physics.Raycast(ray_forward, out hitForward, 2.9f))
        {
            if (hitForward.collider.gameObject.CompareTag("Player") || hitForward.collider.gameObject.name == "Support Ally" || hitForward.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockUp = false;
                if (!enemyHit.Contains(hitForward.collider.gameObject))
                    enemyHit.Add(hitForward.collider.gameObject);
            }

            if (hitForward.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockUp = true;
            }
        }
        else
        {
            enemyMovement.blockUp = false;
        }
        if (Physics.Raycast(ray_back, out hitBack, 2.9f))
        {
            if (hitBack.collider.gameObject.CompareTag("Player") || hitBack.collider.gameObject.name == "Support Ally" || hitBack.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockDown = false;
                if (!enemyHit.Contains(hitBack.collider.gameObject))
                    enemyHit.Add(hitBack.collider.gameObject);
            }
            if (hitBack.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockDown = true;
            }
        }
        else
        {
            enemyMovement.blockDown = false;
        }
        if (Physics.Raycast(ray_right, out hitRight, 2.9f))
        {
            if (hitRight.collider.gameObject.CompareTag("Player") || hitRight.collider.gameObject.name == "Support Ally" || hitRight.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockRight = false;
                if (!enemyHit.Contains(hitRight.collider.gameObject))
                    enemyHit.Add(hitRight.collider.gameObject);
            }
            if (hitRight.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockRight = true;
            }
        }
        else
        {
            enemyMovement.blockRight = false;
            GameObject hitObject = hitRight.collider ? hitRight.collider.gameObject : null;
            if (hitObject != null && enemyHit.Contains(hitObject))
                enemyHit.Remove(hitObject);
        }
        if (Physics.Raycast(ray_left, out hitLeft, 2.9f))
        {
            if (hitLeft.collider.gameObject.CompareTag("Player") || hitLeft.collider.gameObject.name == "Support Ally" || hitLeft.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockLeft = false;
                if (!enemyHit.Contains(hitLeft.collider.gameObject))
                    enemyHit.Add(hitLeft.collider.gameObject);
            }
            if (hitLeft.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockLeft = true;
            }
        }
        else
        {
            enemyMovement.blockLeft = false;
        }
        if (Physics.Raycast(ray_leftforward, out hitLeftForward, 2.9f))
        {
            if (hitLeftForward.collider.gameObject.CompareTag("Player") || hitLeftForward.collider.gameObject.name == "Support Ally" || hitLeftForward.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitLeftForward.collider.gameObject))
                    enemyHit.Add(hitLeftForward.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_leftback, out hitLeftBack, 2.9f))
        {
            if (hitLeftBack.collider.gameObject.CompareTag("Player") || hitLeftBack.collider.gameObject.name == "Support Ally" || hitLeftBack.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitLeftBack.collider.gameObject))
                    enemyHit.Add(hitLeftBack.collider.gameObject);
            }
            if (hitLeftBack.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockLeft = true;
            }
        }
        else
        {
            enemyMovement.blockLeft = false;
        }
        if (Physics.Raycast(ray_rightforward, out hitRightForward, 2.9f))
        {
            if (hitRightForward.collider.gameObject.CompareTag("Player") || hitRightForward.collider.gameObject.name == "Support Ally" || hitRightForward.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitRightForward.collider.gameObject))
                    enemyHit.Add(hitRightForward.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_rightback, out hitRightBack, 2.9f))
        {
            if (hitRightBack.collider.gameObject.CompareTag("Player") || hitRightBack.collider.gameObject.name == "Support Ally" || hitRightBack.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitRightBack.collider.gameObject))
                    enemyHit.Add(hitRightBack.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_forward, out hitForward, 14f))
        {
            if (hitForward.collider.gameObject.CompareTag("Player") || hitForward.collider.gameObject.name == "Support Ally" || hitForward.collider.gameObject.name == "Tank Ally")
                sightedPartyMember = true;
        }
        if (Physics.Raycast(ray_back, out hitBack, 14f))
        {
            if (hitBack.collider.gameObject.CompareTag("Player") || hitBack.collider.gameObject.name == "Support Ally" || hitBack.collider.gameObject.name == "Tank Ally")
                sightedPartyMember = true;
        }
        if (Physics.Raycast(ray_right, out hitRight, 14f))
        {
            if (hitRight.collider.gameObject.CompareTag("Player") || hitRight.collider.gameObject.name == "Support Ally" || hitRight.collider.gameObject.name == "Tank Ally")
                sightedPartyMember = true;
        }
        if (Physics.Raycast(ray_left, out hitLeft, 14f))
        {
            if (hitLeft.collider.gameObject.CompareTag("Player") || hitLeft.collider.gameObject.name == "Support Ally" || hitLeft.collider.gameObject.name == "Tank Ally")
                sightedPartyMember = true;
        }
        if (Physics.Raycast(ray_leftforward, out hitLeftForward, 14f))
        {
            if (hitLeftForward.collider.gameObject.CompareTag("Player") || hitLeftForward.collider.gameObject.name == "Support Ally" || hitLeftForward.collider.gameObject.name == "Tank Ally")
                sightedPartyMember = true;
        }
        if (Physics.Raycast(ray_leftback, out hitLeftBack, 14f))
        {
            if (hitLeftBack.collider.gameObject.CompareTag("Player") || hitLeftBack.collider.gameObject.name == "Support Ally" || hitLeftBack.collider.gameObject.name == "Tank Ally")
                sightedPartyMember = true;
        }
        if (Physics.Raycast(ray_rightforward, out hitRightForward, 14f))
        {
            if (hitRightForward.collider.gameObject.CompareTag("Player") || hitRightForward.collider.gameObject.name == "Support Ally" || hitRightForward.collider.gameObject.name == "Tank Ally")
                sightedPartyMember = true;
        }
        if (Physics.Raycast(ray_rightback, out hitRightBack, 14f))
        {
            if (hitRightBack.collider.gameObject.CompareTag("Player") || hitRightBack.collider.gameObject.name == "Support Ally" || hitRightBack.collider.gameObject.name == "Tank Ally")
                sightedPartyMember = true;
        }
        if (enemyHit.Count > 0)
        {
            for (int i = enemyHit.Count - 1; i >= 0; i--)
            {
                if (!Physics.Raycast(ray_forward, out hitForward, 2.9f) && !Physics.Raycast(ray_back, out hitBack, 2.9f) && !Physics.Raycast(ray_left, out hitLeft, 2.9f) && !Physics.Raycast(ray_right, out hitRight, 2.9f) && !Physics.Raycast(ray_leftforward, out hitLeftForward, 2.9f) && !Physics.Raycast(ray_leftback, out hitLeftBack, 2.9f) && !Physics.Raycast(ray_rightforward, out hitRightForward, 2.9f) && !Physics.Raycast(ray_rightback, out hitRightBack, 2.9f))
                {
                    enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_forward, out hitForward, 2.9f))
                {
                    if (hitForward.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_back, out hitBack, 2.9f))
                {
                    if (hitBack.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_right, out hitRight, 2.9f))
                {
                    if (hitRight.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_left, out hitLeft, 2.9f))
                {
                    if (hitLeft.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_leftforward, out hitLeftForward, 2.9f))
                {
                    if (hitLeftForward.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_leftback, out hitLeftBack, 2.9f))
                {
                    if (hitLeftBack.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_rightforward, out hitRightForward, 2.9f))
                {
                    if (hitRightForward.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_rightback, out hitRightBack, 2.9f))
                {
                    if (hitRightBack.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (hitForward.collider.gameObject.name != enemyHit[i].name)
                {
                    // Delete object from list.
                    enemyHit.RemoveAt(i);
                }
            }
        }
    }
    void RepairEnemy()
    {
        if (enemyStats.hasBeenAttacked)
        {
            if (Physics.Raycast(ray_forward, out hitForward, 2.9f))
            {
                if (hitForward.collider.gameObject.CompareTag("Player") || hitForward.collider.gameObject.name == "Support Ally" || hitForward.collider.gameObject.name == "Tank Ally")
                {
                    enemyAttack.canAttack = true;
                    enemyMovement.blockUp = false;
                    if (!enemyHit.Contains(hitForward.collider.gameObject))
                        enemyHit.Add(hitForward.collider.gameObject);
                }

                if (hitForward.collider.gameObject.CompareTag("Block"))
                {
                    enemyMovement.blockUp = true;
                }
            }
            else
            {
                enemyMovement.blockUp = false;
            }
            if (Physics.Raycast(ray_back, out hitBack, 2.9f))
            {
                if (hitBack.collider.gameObject.CompareTag("Player") || hitBack.collider.gameObject.name == "Support Ally" || hitBack.collider.gameObject.name == "Tank Ally")
                {
                    enemyAttack.canAttack = true;
                    enemyMovement.blockDown = false;
                    if (!enemyHit.Contains(hitBack.collider.gameObject))
                        enemyHit.Add(hitBack.collider.gameObject);
                }
                if (hitBack.collider.gameObject.CompareTag("Block"))
                {
                    enemyMovement.blockDown = true;
                }
            }
            else
            {
                enemyMovement.blockDown = false;
            }
            if (Physics.Raycast(ray_right, out hitRight, 2.9f))
            {
                if (hitRight.collider.gameObject.CompareTag("Player") || hitRight.collider.gameObject.name == "Support Ally" || hitRight.collider.gameObject.name == "Tank Ally")
                {
                    enemyAttack.canAttack = true;
                    enemyMovement.blockRight = false;
                    if (!enemyHit.Contains(hitRight.collider.gameObject))
                        enemyHit.Add(hitRight.collider.gameObject);
                }
                if (hitRight.collider.gameObject.CompareTag("Block"))
                {
                    enemyMovement.blockRight = true;
                }
            }
            else
            {
                enemyMovement.blockRight = false;
                GameObject hitObject = hitRight.collider ? hitRight.collider.gameObject : null;
                if (hitObject != null && enemyHit.Contains(hitObject))
                    enemyHit.Remove(hitObject);
            }
            if (Physics.Raycast(ray_left, out hitLeft, 2.9f))
            {
                if (hitLeft.collider.gameObject.CompareTag("Player") || hitLeft.collider.gameObject.name == "Support Ally" || hitLeft.collider.gameObject.name == "Tank Ally")
                {
                    enemyAttack.canAttack = true;
                    enemyMovement.blockLeft = false;
                    if (!enemyHit.Contains(hitLeft.collider.gameObject))
                        enemyHit.Add(hitLeft.collider.gameObject);
                }
                if (hitLeft.collider.gameObject.CompareTag("Block"))
                {
                    enemyMovement.blockLeft = true;
                }
            }
            else
            {
                enemyMovement.blockLeft = false;
            }
            if (Physics.Raycast(ray_leftforward, out hitLeftForward, 2.9f))
            {
                if (hitLeftForward.collider.gameObject.CompareTag("Player") || hitLeftForward.collider.gameObject.name == "Support Ally" || hitLeftForward.collider.gameObject.name == "Tank Ally")
                {
                    enemyAttack.canAttack = true;
                    if (!enemyHit.Contains(hitLeftForward.collider.gameObject))
                        enemyHit.Add(hitLeftForward.collider.gameObject);
                }
            }
            if (Physics.Raycast(ray_leftback, out hitLeftBack, 2.9f))
            {
                if (hitLeftBack.collider.gameObject.CompareTag("Player") || hitLeftBack.collider.gameObject.name == "Support Ally" || hitLeftBack.collider.gameObject.name == "Tank Ally")
                {
                    enemyAttack.canAttack = true;
                    if (!enemyHit.Contains(hitLeftBack.collider.gameObject))
                        enemyHit.Add(hitLeftBack.collider.gameObject);
                }
                if (hitLeftBack.collider.gameObject.CompareTag("Block"))
                {
                    enemyMovement.blockLeft = true;
                }
            }
            else
            {
                enemyMovement.blockLeft = false;
            }
            if (Physics.Raycast(ray_rightforward, out hitRightForward, 2.9f))
            {
                if (hitRightForward.collider.gameObject.CompareTag("Player") || hitRightForward.collider.gameObject.name == "Support Ally" || hitRightForward.collider.gameObject.name == "Tank Ally")
                {
                    enemyAttack.canAttack = true;
                    if (!enemyHit.Contains(hitRightForward.collider.gameObject))
                        enemyHit.Add(hitRightForward.collider.gameObject);
                }
            }
            if (Physics.Raycast(ray_rightback, out hitRightBack, 2.9f))
            {
                if (hitRightBack.collider.gameObject.CompareTag("Player") || hitRightBack.collider.gameObject.name == "Support Ally" || hitRightBack.collider.gameObject.name == "Tank Ally")
                {
                    enemyAttack.canAttack = true;
                    if (!enemyHit.Contains(hitRightBack.collider.gameObject))
                        enemyHit.Add(hitRightBack.collider.gameObject);
                }
            }
            if (enemyHit.Count > 0)
            {
                for (int i = enemyHit.Count - 1; i >= 0; i--)
                {
                    if (!Physics.Raycast(ray_forward, out hitForward, 2.9f) && !Physics.Raycast(ray_back, out hitBack, 2.9f) && !Physics.Raycast(ray_left, out hitLeft, 2.9f) && !Physics.Raycast(ray_right, out hitRight, 2.9f) && !Physics.Raycast(ray_leftforward, out hitLeftForward, 2.9f) && !Physics.Raycast(ray_leftback, out hitLeftBack, 2.9f) && !Physics.Raycast(ray_rightforward, out hitRightForward, 2.9f) && !Physics.Raycast(ray_rightback, out hitRightBack, 2.9f))
                    {
                        enemyHit.RemoveAt(i);
                    }
                    else if (Physics.Raycast(ray_forward, out hitForward, 2.9f))
                    {
                        if (hitForward.collider.gameObject.name != enemyHit[i].name)
                            enemyHit.RemoveAt(i);
                    }
                    else if (Physics.Raycast(ray_back, out hitBack, 2.9f))
                    {
                        if (hitBack.collider.gameObject.name != enemyHit[i].name)
                            enemyHit.RemoveAt(i);
                    }
                    else if (Physics.Raycast(ray_right, out hitRight, 2.9f))
                    {
                        if (hitRight.collider.gameObject.name != enemyHit[i].name)
                            enemyHit.RemoveAt(i);
                    }
                    else if (Physics.Raycast(ray_left, out hitLeft, 2.9f))
                    {
                        if (hitLeft.collider.gameObject.name != enemyHit[i].name)
                            enemyHit.RemoveAt(i);
                    }
                    else if (Physics.Raycast(ray_leftforward, out hitLeftForward, 2.9f))
                    {
                        if (hitLeftForward.collider.gameObject.name != enemyHit[i].name)
                            enemyHit.RemoveAt(i);
                    }
                    else if (Physics.Raycast(ray_leftback, out hitLeftBack, 2.9f))
                    {
                        if (hitLeftBack.collider.gameObject.name != enemyHit[i].name)
                            enemyHit.RemoveAt(i);
                    }
                    else if (Physics.Raycast(ray_rightforward, out hitRightForward, 2.9f))
                    {
                        if (hitRightForward.collider.gameObject.name != enemyHit[i].name)
                            enemyHit.RemoveAt(i);
                    }
                    else if (Physics.Raycast(ray_rightback, out hitRightBack, 2.9f))
                    {
                        if (hitRightBack.collider.gameObject.name != enemyHit[i].name)
                            enemyHit.RemoveAt(i);
                    }
                    else if (hitForward.collider.gameObject.name != enemyHit[i].name)
                    {
                        // Delete object from list.
                        enemyHit.RemoveAt(i);
                    }
                }
            }
        }
    }
    void BossEnemy()
    {
        if (Physics.Raycast(ray_forward,out hitForward, 8f))
        {
            if (hitForward.collider.gameObject.CompareTag("Player") || hitForward.collider.gameObject.name == "Support Ally" || hitForward.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canRangedAttack = true;
                enemyMovement.isInRange = true;
                if (!enemyHit.Contains(hitForward.collider.gameObject))
                    enemyHit.Add(hitForward.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_back, out hitBack, 8f))
        {
            if (hitBack.collider.gameObject.CompareTag("Player") || hitBack.collider.gameObject.name == "Support Ally" || hitBack.collider.gameObject.name == "Tank Ally")
            {
                enemyMovement.isInRange = true;
                enemyAttack.canRangedAttack = true;
                if (!enemyHit.Contains(hitBack.collider.gameObject))
                    enemyHit.Add(hitBack.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_left, out hitLeft, 8f))
        {
            if (hitLeft.collider.gameObject.CompareTag("Player") || hitLeft.collider.gameObject.name == "Support Ally" || hitLeft.collider.gameObject.name == "Tank Ally")
            {
                enemyMovement.isInRange = true;
                enemyAttack.canRangedAttack = true;
                if (!enemyHit.Contains(hitLeft.collider.gameObject))
                    enemyHit.Add(hitLeft.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_right, out hitRight, 8f))
        {
            if (hitRight.collider.gameObject.CompareTag("Player") || hitRight.collider.gameObject.name == "Support Ally" || hitRight.collider.gameObject.name == "Tank Ally")
            {
                enemyMovement.isInRange = true;
                enemyAttack.canRangedAttack = true;
                if (!enemyHit.Contains(hitRight.collider.gameObject))
                    enemyHit.Add(hitRight.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_leftforward, out hitLeftForward, 8f))
        {
            if (hitLeftForward.collider.gameObject.CompareTag("Player") || hitLeftForward.collider.gameObject.name == "Support Ally" || hitLeftForward.collider.gameObject.name == "Tank Ally")
            {
                enemyMovement.isInRange = true;
                enemyAttack.canRangedAttack = true;
                if (!enemyHit.Contains(hitLeftForward.collider.gameObject))
                    enemyHit.Add(hitLeftForward.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_leftback, out hitLeftBack, 8f))
        {
            if (hitLeftBack.collider.gameObject.CompareTag("Player") || hitLeftBack.collider.gameObject.name == "Support Ally" || hitLeftBack.collider.gameObject.name == "Tank Ally")
            {
                enemyMovement.isInRange = true;
                enemyAttack.canRangedAttack = true;
                if (!enemyHit.Contains(hitLeftBack.collider.gameObject))
                    enemyHit.Add(hitLeftBack.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_rightforward, out hitRightForward, 8f))
        {
            if (hitRightForward.collider.gameObject.CompareTag("Player") || hitRightForward.collider.gameObject.name == "Support Ally" || hitRightForward.collider.gameObject.name == "Tank Ally")
            {
                enemyMovement.isInRange = true;
                enemyAttack.canRangedAttack = true;
                if (!enemyHit.Contains(hitRightForward.collider.gameObject))
                    enemyHit.Add(hitRightForward.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_rightback, out hitRightBack, 8f))
        {
            if (hitRightBack.collider.gameObject.CompareTag("Player") || hitRightBack.collider.gameObject.name == "Support Ally" || hitRightBack.collider.gameObject.name == "Tank Ally")
            {
                enemyMovement.isInRange = true;
                enemyAttack.canRangedAttack = true;
                if (!enemyHit.Contains(hitRightBack.collider.gameObject))
                    enemyHit.Add(hitRightBack.collider.gameObject);
            }
        }

        if (Physics.Raycast(ray_forward, out hitForward, 2f))
        {
            if (hitForward.collider.gameObject.CompareTag("Player") || hitForward.collider.gameObject.name == "Support Ally" || hitForward.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockUp = false;
                if (!enemyHit.Contains(hitForward.collider.gameObject))
                    enemyHit.Add(hitForward.collider.gameObject);
            }

            if (hitForward.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockUp = true;
            }
        }
        else
        {
            enemyMovement.blockUp = false;
        }
        if (Physics.Raycast(ray_back, out hitBack, 2f))
        {
            if (hitBack.collider.gameObject.CompareTag("Player") || hitBack.collider.gameObject.name == "Support Ally" || hitBack.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockDown = false;
                if (!enemyHit.Contains(hitBack.collider.gameObject))
                    enemyHit.Add(hitBack.collider.gameObject);
            }
            if (hitBack.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockDown = true;
            }
        }
        else
        {
            enemyMovement.blockDown = false;
        }
        if (Physics.Raycast(ray_right, out hitRight, 2f))
        {
            if (hitRight.collider.gameObject.CompareTag("Player") || hitRight.collider.gameObject.name == "Support Ally" || hitRight.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockRight = false;
                if (!enemyHit.Contains(hitRight.collider.gameObject))
                    enemyHit.Add(hitRight.collider.gameObject);
            }
            if (hitRight.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockRight = true;
            }
        }
        else
        {
            enemyMovement.blockRight = false;
            GameObject hitObject = hitRight.collider ? hitRight.collider.gameObject : null;
            if (hitObject != null && enemyHit.Contains(hitObject))
                enemyHit.Remove(hitObject);
        }
        if (Physics.Raycast(ray_left, out hitLeft, 2f))
        {
            if (hitLeft.collider.gameObject.CompareTag("Player") || hitLeft.collider.gameObject.name == "Support Ally" || hitLeft.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                enemyMovement.blockLeft = false;
                if (!enemyHit.Contains(hitLeft.collider.gameObject))
                    enemyHit.Add(hitLeft.collider.gameObject);
            }
            if (hitLeft.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockLeft = true;
            }
        }
        else
        {
            enemyMovement.blockLeft = false;
        }
        if (Physics.Raycast(ray_leftforward, out hitLeftForward, 2f))
        {
            if (hitLeftForward.collider.gameObject.CompareTag("Player") || hitLeftForward.collider.gameObject.name == "Support Ally" || hitLeftForward.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitLeftForward.collider.gameObject))
                    enemyHit.Add(hitLeftForward.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_leftback, out hitLeftBack, 2f))
        {
            if (hitLeftBack.collider.gameObject.CompareTag("Player") || hitLeftBack.collider.gameObject.name == "Support Ally" || hitLeftBack.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitLeftBack.collider.gameObject))
                    enemyHit.Add(hitLeftBack.collider.gameObject);
            }
            if (hitLeftBack.collider.gameObject.CompareTag("Block"))
            {
                enemyMovement.blockLeft = true;
            }
        }
        else
        {
            enemyMovement.blockLeft = false;
        }
        if (Physics.Raycast(ray_rightforward, out hitRightForward, 2f))
        {
            if (hitRightForward.collider.gameObject.CompareTag("Player") || hitRightForward.collider.gameObject.name == "Support Ally" || hitRightForward.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitRightForward.collider.gameObject))
                    enemyHit.Add(hitRightForward.collider.gameObject);
            }
        }
        if (Physics.Raycast(ray_rightback, out hitRightBack, 2f))
        {
            if (hitRightBack.collider.gameObject.CompareTag("Player") || hitRightBack.collider.gameObject.name == "Support Ally" || hitRightBack.collider.gameObject.name == "Tank Ally")
            {
                enemyAttack.canAttack = true;
                if (!enemyHit.Contains(hitRightBack.collider.gameObject))
                    enemyHit.Add(hitRightBack.collider.gameObject);
            }
        }
        if (enemyHit.Count > 0)
        {
            for (int i = enemyHit.Count - 1; i >= 0; i--)
            {
                if (!Physics.Raycast(ray_forward, out hitForward, 8f) && !Physics.Raycast(ray_back, out hitBack, 2.9f) && !Physics.Raycast(ray_left, out hitLeft, 2.9f) && !Physics.Raycast(ray_right, out hitRight, 2.9f) && !Physics.Raycast(ray_leftforward, out hitLeftForward, 2.9f) && !Physics.Raycast(ray_leftback, out hitLeftBack, 2.9f) && !Physics.Raycast(ray_rightforward, out hitRightForward, 2.9f) && !Physics.Raycast(ray_rightback, out hitRightBack, 2.9f))
                {
                    enemyMovement.isInRange = false;
                    enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_forward, out hitForward, 8f))
                {
                    enemyMovement.isInRange = false;
                    if (hitForward.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_back, out hitBack, 8f))
                {
                    enemyMovement.isInRange = false;
                    if (hitBack.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_right, out hitRight, 8f))
                {
                    enemyMovement.isInRange = false;
                    if (hitRight.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_left, out hitLeft, 8f))
                {
                    enemyMovement.isInRange = false;
                    if (hitLeft.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_leftforward, out hitLeftForward, 8f))
                {
                    enemyMovement.isInRange = false;
                    if (hitLeftForward.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_leftback, out hitLeftBack, 8f))
                {
                    enemyMovement.isInRange = false;
                    if (hitLeftBack.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_rightforward, out hitRightForward, 8f))
                {
                    enemyMovement.isInRange = false;
                    if (hitRightForward.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (Physics.Raycast(ray_rightback, out hitRightBack, 8f))
                {
                    enemyMovement.isInRange = false;
                    if (hitRightBack.collider.gameObject.name != enemyHit[i].name)
                        enemyHit.RemoveAt(i);
                }
                else if (hitForward.collider.gameObject.name != enemyHit[i].name)
                {
                    enemyMovement.isInRange = false;
                    // Delete object from list.
                    enemyHit.RemoveAt(i);
                }
            }
        }
    }
}
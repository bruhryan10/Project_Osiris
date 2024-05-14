using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestDetect : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 target;

    public float detectionRange = 10f;
    Transform player;
    public bool isChasing = false;

   // public AnimationClip walkAnimationClip;
   // private Animation animationComponent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

       // animationComponent = GetComponent<Animation>();
       // animationComponent.AddClip(walkAnimationClip, "basic barbarian|walk");
    }

   
    void Start()
    {
        Bruh();
        UpdateDestination();
    }

    private void OnEnable()
    {
        Bruh();
    }

    void Bruh()
    {
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
    }

    
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            agent.SetDestination(player.position);
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                UpdateDestination();
            }
            else if (!isChasing && !agent.pathPending && agent.remainingDistance < 0.1f)
            {
                IterateWaypointIndex();
                UpdateDestination();
            }
        }
    }

    void UpdateDestination()
    {
        if (!isChasing)
        {
            target = waypoints[waypointIndex].position;
            agent.SetDestination(target);
        }
    }

    void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
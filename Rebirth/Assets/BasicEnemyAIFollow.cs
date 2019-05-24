using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAIFollow : MonoBehaviour
{
    // variables for following player
    Transform goal;          // transform of the player
    NavMeshAgent agent;      // navmesh agent component of enemy

    void Start()
    {
        // assign relevant data
        goal = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // set the destination of the navmesh agent
        agent.SetDestination(goal.position);
    }
}
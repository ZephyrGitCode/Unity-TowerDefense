using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMover : MonoBehaviour
{
    [Tooltip("Goal as set by EnemySpawner")]
    public Transform goal;
    private NavMeshAgent agent;
    private Enemy enemy;

    private void Awake() {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void SetDestination(Transform givenGoal)
    {
        goal = givenGoal;
        agent.destination = goal.position;
    }

    private void Update() {
        /*
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(goal.position, path);
        if (path.status == NavMeshPathStatus.PathPartial)
        {
            Debug.Log("Cannot make the path");
        }
        */

        if (goal != null && agent.remainingDistance < 1f)
        {
            // TODO take life
            enemy.TakeLife();
        }
    }
}

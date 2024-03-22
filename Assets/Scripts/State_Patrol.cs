using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class State_Patrol : FSM_Base
{
    public List<Transform> patrolPoints = new List<Transform>();
    int currentPatrolPoint = 0;
    public float minStoppingDistance = 0.1f;
    public float distanceCheckFrequency = 0.1f;
    float nextDistanceCheck = 0;
    public float waitTime = 0.3f;
    bool waiting = false;
    private float nextMoveTime = 0;


    internal override void OnStateEnterArgs()
    {
        if(patrolPoints.Count > 0)
        {
            int closestPoint = 0;
            float closestDistance = Vector3.Distance(transform.position, patrolPoints[0].position);
            for (int i = 0; i < patrolPoints.Count; i++)
            {
                float currentDist = Vector3.Distance(transform.position, patrolPoints[i].position);
                if(currentDist < closestDistance)
                {
                    closestPoint = i;
                    closestDistance = currentDist;
                }
            }
            currentPatrolPoint = closestPoint;
            brain.AssignTarget(patrolPoints[currentPatrolPoint].gameObject, false);
            brain.MoveToTarget();
        }
    }
    public override void UpdateState()
    {
        if(waiting && Time.time >= nextMoveTime)
        {
            //let's move
            waiting = false;
            GoToNextPatrolPoint();
            nextDistanceCheck = Time.time + distanceCheckFrequency;

        }
        else if(!waiting && Time.time >= nextDistanceCheck)
        {
            if(brain.getDistanceToDestination() <= minStoppingDistance) // if we are close enough to stop
            {
                waiting = true;
                nextMoveTime = Time.time + waitTime;
            }
            else
            {
                nextDistanceCheck = Time.time + distanceCheckFrequency; // if we are not close enough to stop
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        currentPatrolPoint++;
        if(currentPatrolPoint >= patrolPoints.Count)
        {
            currentPatrolPoint = 0;
        }
        brain.AssignTarget(patrolPoints[currentPatrolPoint].gameObject, false);
        brain.MoveToTarget();
    }
}

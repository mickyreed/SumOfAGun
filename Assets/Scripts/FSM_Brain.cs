using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSM_Brain : MonoBehaviour
{
    public FSM_Base initialState;
    FSM_Base currentState;

    [SerializeField]
    NavMeshAgent agent;

    public GameObject currentTarget;
    public bool targetIsPlayer = false;

    void Start()
    {
        FSM_Base[] states = GetComponents<FSM_Base>(); //creates an array of every component of this type thats found on the game object
        
        for (int i = 0; i < states.Length; i++)
        {
            states[i].isActive = false;
            states[i].AssignBrain(this);
        }
        if (initialState == null)
        {
            currentState = states[0];
        }
        else
        {
            currentState = initialState;
        }

        //currentState = initialState;
        currentState.OnStateEnter();
    }

    public float getDistanceToDestination()
    {
        float distance = 0f;
        for (int i = 0; i < agent.path.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(agent.path.corners[i], agent.path.corners[i+1]);
            //agent.remainingDistance = distance; // dont use this not reliable
        }
        return distance;
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void MoveToTarget()
    {
        agent.SetDestination(currentTarget.transform.position);
    }

    public void AssignTarget(GameObject target, bool isPlayer)
    {
        currentTarget = target;
        targetIsPlayer = isPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }
}

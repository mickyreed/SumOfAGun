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

    bool paused = false;

    [Header("Player Detection")]
    public float detectionRadius = 20f;
    public LayerMask enemyMask;
    bool targetInRange = false;
    public CapsuleCollider visibilityCapsule;


    void Start()
    {
        PauseControl.instance.pause += (paused) => {this.paused = paused;};
        
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
        if (paused)
        {
            return;
        }
        currentState.UpdateState();
    }

    Vector3 ReturnCapsulePoint(bool isOne, CapsuleCollider refCollider)
    {
        Vector3 point = refCollider.bounds.center;
        //Vector3 point = transform.position; // this is also valid
        switch (refCollider.direction)
        {
            case 0: //x
                point += new Vector3(isOne ? 0 + refCollider.bounds.extents.x : 0 - refCollider.bounds.extents.x, 0f, 0f); //ifOne == true?
                break;

            case 1: //y
                point += new Vector3(0f, isOne ? 0 + refCollider.bounds.extents.y : 0 - refCollider.bounds.extents.y, 0f);
                break;

            case 2: //z
                point += new Vector3(0f, 0f, isOne? 0 + refCollider.bounds.extents.y : 0 - refCollider.bounds.extents.z);
                break;
        }
        return point;
    }

    public bool DetectPlayer()
    {
        Collider[] playerColls = Physics.OverlapSphere(transform.position, detectionRadius, enemyMask);
        if(playerColls.Length > 0 )
        {
            //Physics.CapsuleCast()
            targetInRange = true;
            RaycastHit hit;
            if(Physics.CapsuleCast(ReturnCapsulePoint(false, visibilityCapsule),
                ReturnCapsulePoint(true, visibilityCapsule),
                visibilityCapsule.radius,
                (playerColls[0].transform.position - transform.position).normalized,
                out hit, Vector3.Distance(playerColls[0].transform.position, transform.position) +0.1f,
                enemyMask))
            {
                targetIsPlayer = true;
                if(hit.collider.transform.parent != null)
                {
                    currentTarget = hit.collider.transform.parent.gameObject;
                }
                else
                {
                    currentTarget = hit.collider.gameObject;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


}

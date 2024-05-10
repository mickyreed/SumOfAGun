using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;

public class FSM_Brain : MonoBehaviour
{
    public FSM_Base initialState;
    FSM_Base currentState;

    [SerializeField]
    NavMeshAgent agent;

    public GameObject currentTarget;
    public bool targetIsPlayer = false;
    public EnemyCombatControl combatControl;
    bool paused = false;

    [Header("Death")]
    public bool hasDeathIMplemented = false;
    public float deathDeleteDelay = 3.3f;
    public List<GameObject> deathObjDestroyList = new List<GameObject>();
    public List<Component> deathComponentDestroyList = new List<Component>();
    float deathDeleteTime = 0f;
    bool dead = false;

    float timeOfLastHit = 0f;
    public FSM_Base hurtState;
    public Animator animator;
    [Header("Player Detection")]
    public float detectionRadius = 20f;
    public LayerMask enemyMask;
    public LayerMask obstacleMask;
    bool targetInRange = false;
    //bool targetVisible = false;
    public CapsuleCollider visibilityCapsule;
    public EventTypes.VoidDel soundHeard;
    public GameObject tempTargetPrefab;
    GameObject currentTempTarget;

    void Start()
    {
        PauseControl.instance.pause += (paused) => { this.paused = paused; };

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

    public void ExecuteMeleeAttack(MeleeAttackInfo attackInfo)
    {
        animator.Play(attackInfo.name);
        //assign damage info to the hurtbox
    }


    public void StartFire(bool misfire)
    {
        combatControl.Fire(misfire);
    }
    public void EndFire()
    {
        combatControl.EndFire();
    }

    public GunData ReturnCurrentWeapon()
    {
        return combatControl.currentWeaponData;
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

    public void StopOnSpot()
    {
        agent.SetDestination(transform.position);
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
    public void RecieveNewState(FSM_Base newState)
    {
        currentState = newState;
    }
    
    // Update is called once per frame
    
    void Update()
    {
        if (paused )
        {
            return;
        }
        if (dead)
        {
            if(Time.time >= deathDeleteTime)
            {
                Destroy(gameObject);
            }
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
            case 0: //x  //isOne ? checks if ifOne is true  - if it is do first condition else do second condition
                point += new Vector3(isOne ? 0 + refCollider.bounds.extents.x : 0 - refCollider.bounds.extents.x, 0f, 0f); //ifOne == true?
                break;

            case 1: //y
                point += new Vector3(0f, isOne ? 0 + refCollider.bounds.extents.y : 0 - refCollider.bounds.extents.y, 0f);
                break;

            case 2: //z
                point += new Vector3(0f, 0f, isOne? 0 + refCollider.bounds.extents.z : 0 - refCollider.bounds.extents.z);
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
            //targetVisible = true;
            RaycastHit hit;
            if(Physics.CapsuleCast(ReturnCapsulePoint(false, visibilityCapsule),
                ReturnCapsulePoint(true, visibilityCapsule),
                visibilityCapsule.radius,
                (playerColls[0].transform.position - transform.position).normalized,
                out hit, 
                Vector3.Distance(playerColls[0].transform.position, transform.position) +0.1f,
                obstacleMask))
            {
                targetIsPlayer = true;
                if(playerColls[0].transform.parent != null)
                {
                    currentTarget = playerColls[0].transform.parent.gameObject;
                }
                else
                {
                    currentTarget = playerColls[0].gameObject;
                }
                if(currentTempTarget != null)
                {
                    Destroy(currentTempTarget);
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

    public bool CanHit()
    {
        if(Time.time >= timeOfLastHit + CalculateTimeUntilNextHit())
        {
            print("Can hit");
            timeOfLastHit = Time.time;
            return true;
        }
        else
        {
            print("Cannot hit");
            return false;
        }
    }

    float CalculateTimeUntilNextHit()
    {
        float farDistanceMod = 15;
        float nearDistanceMod = 5;

        float distanceMod = 1 + Mathf.Clamp01((
            Vector3.Distance(currentTarget.transform.position, transform.position) 
            - nearDistanceMod)/(farDistanceMod - nearDistanceMod)) *3;
        print("distance mod: " + distanceMod);

        Vector3 playerMove = currentTarget.GetComponent<PlayerMovement>().CurrentMovement;
        playerMove.y = 0f;
        float peakAngle = 90f;
        float lateralAngle = Vector3.Angle(-transform.forward, playerMove);

        float deviation = 1 + (lateralAngle > 90 ? 1 - (Mathf.Abs(lateralAngle - peakAngle) / peakAngle)
            : Mathf.Abs(lateralAngle - peakAngle) / peakAngle) *3;

        // example1: lateralAngle = 47
        // (47 - 90)/90 = -43/90
        // and being absolute it becomes 43/90c - which equals .477

        // example2: lateralAngle = 110
        // (110 -90)/90 = 20/90 - which equals 0.222
        // which becomes 1 - 0.222 - which equals 0.778

        return 0.5f * (distanceMod * deviation);
    }

    public void HearSound(Vector3 soundPos)
    {
        if (!targetIsPlayer)
        {
            currentTempTarget = Instantiate(tempTargetPrefab, soundPos, Quaternion.identity);
            currentTarget = currentTempTarget;
            targetIsPlayer = false;

            //transition to chase state
            soundHeard?.Invoke(); // will run the delegate
        }
        //currentTempTarget = Instantiate(tempTargetPrefab, soundPos, Quaternion.identity);
    }
    public void TookDamage()
    {
        if(hurtState != null)
        {
            currentState.TransitionToNextState(hurtState);


        }
    }

    public void Die()
    {
        //Destroy(gameObject);
        if (!hasDeathIMplemented)
        {
            Destroy(gameObject);
        }
        else
        {
            dead = true;
            deathDeleteTime = Time.time + deathDeleteDelay;
            if(currentTempTarget != null)
            {
                Destroy(currentTempTarget);
            }
            animator?.SetTrigger("Death");
            for (int i = 0; i < deathObjDestroyList.Count; i++)
            {
                Destroy(deathObjDestroyList[i]);
            }
            for (int i = 0; i < deathComponentDestroyList.Count; i++)
            {
                Destroy(deathComponentDestroyList[i]);
            }
        }
        
        
    }

}

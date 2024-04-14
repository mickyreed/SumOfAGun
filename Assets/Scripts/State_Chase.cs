using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Chase : FSM_Base
{
    public float checkInterval = 0.1f;
    float nextCheck = 0f;
    public List<Transition_Range> rangeTransitions = new List<Transition_Range>();

    internal override void OnStateEnterArgs()
    {
        brain.MoveToTarget();
        nextCheck = Time.time + checkInterval + Random.Range(0.001f, 0.05f); // add random to make similar enemies move differently
    }
    public override void UpdateState()
    {
        if (Time.time >= nextCheck)
        {
            if(rangeTransitions.Count > 0)
            {
                int closestState = -1;
                //can do both ranged and melee attack here
                float closestDistance = rangeTransitions[0].range + 1;
                for (int i = 0; i < rangeTransitions.Count; i++)
                {
                    float currentRange = rangeTransitions[i].range;
                    //this will find the closest state out of all of these
                    // if any are valid the closest will be not be equal to -1
                    if (brain.getDistanceToDestination() <= currentRange && currentRange < closestDistance)
                    {
                        closestDistance = currentRange;
                        closestState = i;
                    }
                }

                if (closestState != -1)
                {
                    // if a transition point is in range we move toward it
                    TransitionToNextState(rangeTransitions[closestState].stateToEnter);
                }
                else
                {
                    // else we move to target
                    brain.MoveToTarget();
                    nextCheck = Time.time + checkInterval;
                }
            }
            else
            {
                brain.MoveToTarget();
                nextCheck = Time.time + checkInterval;
            }
            
        }
        //brain.MoveToTarget();
    }

    internal override void OnStateExitArgs()
    {
        brain.StopOnSpot();
    }
}

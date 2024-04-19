using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This state will wait until it detects the player and then transition to whatever state is assigned as the chase state
/// </summary>
public class State_Idle : FSM_Base
{
    public float checkInterval = 0.3f;
    float nextCheck = 0f;
    public FSM_Base chaseState;

    internal override void OnStateEnterArgs()
    {
        brain.soundHeard += RecieveSoundTarget;
    }

    public override void UpdateState()
    {
        if(Time.time > nextCheck)
        {
            if (brain.DetectPlayer())
            {
                TransitionToNextState(chaseState);
            }
            else
            {
                nextCheck = Time.time + checkInterval;
            }
        }
    }

    void RecieveSoundTarget()
    {
        TransitionToNextState(chaseState);
    }

    internal override void OnStateExitArgs()
    {
        brain.soundHeard -= RecieveSoundTarget;
    }

}
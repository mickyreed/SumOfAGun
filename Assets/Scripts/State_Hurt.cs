using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Hurt : FSM_Base
{
    public FSM_Base retaliateState;
    public FSM_Base searchState;
    public float recoveryTime = 0.5f;
    float recoveryEnd = 0f;
    internal override void OnStateEnterArgs()
    {
        recoveryEnd = Time.time + recoveryTime;
    }

    public override void UpdateState()
    {
        if(Time.time >= recoveryEnd)
        {
            if(brain.currentTarget !=null && brain.targetIsPlayer && retaliateState != null)
            {
                TransitionToNextState(retaliateState);
            }
            else if(brain.currentTarget != null && !brain.targetIsPlayer && searchState != null)
            {
                TransitionToNextState(searchState);
            }
        }
    }

}


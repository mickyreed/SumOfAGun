using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM_Base : MonoBehaviour
{
    public bool isActive = false;
    internal FSM_Brain brain;

    public void OnStateEnter()
    {
        isActive = true;
        OnStateEnterArgs();
    }

    public void AssignBrain(FSM_Brain brain)
    {
        this.brain = brain;
    }

    internal virtual void OnStateEnterArgs()
    {

    }

    public abstract void UpdateState();

    internal void TransitionToNextState(FSM_Base nextState)
    {
        OnStateExit();
        nextState.OnStateExit();
    }

    private void OnStateExit()
    {
        isActive = false;
        OnStateExitArgs();
    }

    internal virtual void OnStateExitArgs()
    {

    }

    

}

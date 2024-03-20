using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM_Base : MonoBehaviour
{
    public bool isActive = false;
    

    public void OnStateEnter()
    {
        isActive = true;
        OnStateEnterArgs();
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

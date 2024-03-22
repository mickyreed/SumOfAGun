using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Chase : FSM_Base
{
    public float checkInterval = 0.1f;
    float nextCheck = 0f;
    internal override void OnStateEnterArgs()
    {
        brain.MoveToTarget();
        nextCheck = Time.time + checkInterval + Random.Range(0.001f, 0.05f); // add random to make similar enemies move differently
    }
    public override void UpdateState()
    {
        if (Time.time >= nextCheck)
        {
            brain.MoveToTarget();
        }
        //brain.MoveToTarget();
    }
}

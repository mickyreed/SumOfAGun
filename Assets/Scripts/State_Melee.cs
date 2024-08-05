using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Melee : FSM_Base
{
    public int attackId = 0;
    //public float attackInterval
    float nextAttack = 0;
    public Transition_Range outOfRangeTransition;


    internal override void OnStateEnterArgs()
    {
        brain.combatControl.MeleAttack(attackId);
        nextAttack = Time.time + brain.combatControl.currentWeaponData.fireRate;
    }

    public override void UpdateState()
    {
        if(Time.time >= nextAttack)
        {
            if (Vector3.Distance(transform.position, brain.currentTarget.transform.position) >= outOfRangeTransition.range)
            {
                TransitionToNextState(outOfRangeTransition.stateToEnter);
            }
            else
            {
                brain.combatControl.MeleAttack(attackId);
                nextAttack = Time.time + brain.combatControl.currentWeaponData.fireRate;
            }
            
        }
        
    }

}


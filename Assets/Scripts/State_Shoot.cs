using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Shoot : FSM_Base
{
    public float lookSpeed = 20f;
    float nextShot = 0f;

    public override void UpdateState()
    {
        Vector3 targetDir = (brain.currentTarget.transform.position - transform.position).normalized;
        float degreeDifference = Vector3.SignedAngle(transform.forward, targetDir, transform.up);
        transform.Rotate(transform.up, 
            degreeDifference < lookSpeed * Time.deltaTime ? 
            degreeDifference : lookSpeed * Mathf.Sign(degreeDifference)*Time.deltaTime);
        
        if(Time.time >= nextShot)
        {
            brain.combatControl.Fire(brain.CanHit());
            nextShot = Time.time + brain.ReturnCurrentWeapon().fireRate;
        }
        
    }
}

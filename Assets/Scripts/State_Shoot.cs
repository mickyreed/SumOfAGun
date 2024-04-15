using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Shoot : FSM_Base
{
    public float lookSpeed = 20f;
    float nextShot = 0f;

    public bool exitsByLifetime = true;
    public float minLifeTime = 2.5f;
    public float maxLifeTime = 3.5f;
    float lifeTime = 3f;
    float lifeTimeEnd = 0f;

    public bool exitsByShots = false;
    public int maxLifeTimeShots = 3;
    int currentShot = 0;
    public FSM_Base endLifeTImeState;

    internal override void OnStateEnterArgs()
    {
        lifeTime = Random.Range(minLifeTime, maxLifeTime);
        lifeTimeEnd = Time.time + lifeTime;

    }

    public override void UpdateState()
    {
        if(exitsByLifetime && Time.time > lifeTimeEnd ||
            exitsByShots && currentShot >= maxLifeTimeShots)
        {
            TransitionToNextState(endLifeTImeState);
        }

        Vector3 targetDir = (brain.currentTarget.transform.position - transform.position).normalized;
        float degreeDifference = Vector3.SignedAngle(transform.forward, targetDir, transform.up);
        transform.Rotate(transform.up, 
            degreeDifference < lookSpeed * Time.deltaTime ? 
            degreeDifference : lookSpeed * Mathf.Sign(degreeDifference)*Time.deltaTime);
        
        if(Time.time >= nextShot)
        {
            brain.combatControl.Fire(brain.CanHit());
            nextShot = Time.time + brain.ReturnCurrentWeapon().fireRate;
            currentShot++;
        }
        
    }

    internal override void OnStateExitArgs()
    {
        currentShot = 0;
    }
}

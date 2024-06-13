using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxControl : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public float radius = 0.1f;
    public LayerMask hitMask;

    Vector3 lastPos;
    HashSet<GameObject> hitObjs = new HashSet<GameObject>();
    
    Coroutine castRoutine;
    MeleeAttackInfo currentAttack;

    public void AssignMeleeAttackInfo(MeleeAttackInfo currentAttack)
    {
        this.currentAttack = currentAttack;
    }

    public void StartCast()
    {
        lastPos = transform.position;
        castRoutine = StartCoroutine(CastRoutine());
    }

    void Cast()
    {
        RaycastHit hit;
        Vector3 direction = transform.position - lastPos; // (point1.position + ( point2.position - point1.position)*0.5f)); //possible midpoint
        if(Physics.CapsuleCast(point1.position,
            point2.position,
            radius, 
            direction.normalized, 
            out hit, 
            direction.magnitude, 
            hitMask))
        {
            // handle hit detection
            TakeDamageTest hitObj = hit.collider.GetComponent<TakeDamageTest>(); // so if we hit the object and its got a takedamge test on it
            if (hitObj != null && !hitObjs.Contains(hitObj.ReturnParentObj())) // and the hasset doesnt contain the parent object of the colldier we hit
            {
                // hit it and deal damage
                print("melee deal damage");
                hitObj.TakeDamage(currentAttack.damage);
                hitObjs.Add(hitObj.ReturnParentObj());
            }
        }
        lastPos = transform.position;
    }

    IEnumerator CastRoutine()
    {
        while (true)
        {
            Cast();
            yield return null;
        }
    }

    public void EndCast()
    {
        hitObjs.Clear();
        StopCoroutine(castRoutine);
    }
    
}

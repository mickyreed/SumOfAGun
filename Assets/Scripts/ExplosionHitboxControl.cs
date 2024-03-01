using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHitboxControl : MonoBehaviour
{
    public AmmoType type; //for the damage

    HashSet<GameObject> collidedObjs = new HashSet<GameObject>();
    // hasset is like a list but is unordered, you cant use an index to reference - its like a pool so more efficient
    // can use it to check something is in the set

    private void OnTriggerEnter(Collider other)
    {
        TakeDamageTest hitObj = other.GetComponent<TakeDamageTest>(); // so if we hit the object and its got a takedamge test on it
        if (hitObj != null && !collidedObjs.Contains(hitObj.ReturnParentObj())) // and the hasset doesnt contain the parent object of the colldier we hit
        {
            // hit it and deal damage
            hitObj.TakeDamage(type.damage);
            collidedObjs.Add(hitObj.ReturnParentObj());
        }
    }
}

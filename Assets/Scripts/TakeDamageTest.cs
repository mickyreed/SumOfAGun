using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakeDamageTest : MonoBehaviour
{
    public EventTypes.IntEvent tookDamage;
    [SerializeField]
    GameObject parent;

    public GameObject ReturnParentObj() //return parent object to make sure we havent collided with it
    {
        if (parent == null)
        {
            return transform.parent.gameObject;
        }
        else
        {
            return parent;
        }

    }

    public void TakeDamage(int damage)
    {
        print(gameObject.name + " took " +  damage + " points of damage");
        tookDamage.Invoke(damage);
    }
}

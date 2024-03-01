using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageTest : MonoBehaviour
{
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
    }
}

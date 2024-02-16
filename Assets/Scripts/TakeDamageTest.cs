using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageTest : MonoBehaviour
{
   public void TakeDamage(int damage)
    {
        print(gameObject.name + " took " +  damage + " points of damage");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpControl : MonoBehaviour
{
    
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PickUp")
        {
            AmmoPickUp ammoPickUp = other.GetComponent<AmmoPickUp>();


        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPickUpControl : MonoBehaviour
{
    //public EventTypes.IntEvent ammoFoundEvent;
    public EventTypes.IntAmmoEvent ammoFoundEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PickUp")
        {
            AmmoPickUp ammoPickUp = other.GetComponent<AmmoPickUp>();
            
            if(ammoPickUp != null)
            {
                ammoFoundEvent.Invoke(ammoPickUp.ammo, ammoPickUp.ammoType);
                ammoPickUp.Clear();
            }
            
        }
    }
}

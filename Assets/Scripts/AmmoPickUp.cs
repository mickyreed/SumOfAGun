using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    public AmmoType ammoType;
    public int ammo = 50;
    public void Clear()
    {
        Destroy(gameObject);
    }
}

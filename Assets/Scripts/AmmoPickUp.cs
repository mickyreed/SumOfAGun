using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    private AudioManager audioManager;

    public AmmoType ammoType;
    public int ammo = 50;
    public void Clear()
    {
        audioManager.PlaySound(audioManager.ammoPickupSound);
        Destroy(gameObject);
    }

    private void Start()
    {
        audioManager = AudioManager.instance;
    }
}

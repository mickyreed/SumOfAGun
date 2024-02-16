using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponControl : MonoBehaviour
{
    public Camera viewCamera;
    public LayerMask hitMask;
    public GunData currentWeaponData;
    public GunControl currentGun;

    public List<AmmoType> AmmoTypes = new List<AmmoType>();
    public Dictionary<AmmoType, int> ammoStore = new Dictionary<AmmoType, int>();
    
    bool recoiling = false;
    float nextFireTime = 0;

    Coroutine automaticRoutine;
    
    // Start is called before the first frame update
    void Start()
    {
        InitialiseAmmo();
    }

    void InitialiseAmmo()
    {
        for(int i = 0; i < AmmoTypes.Count; i++)
        {
            ammoStore.Add(AmmoTypes[i], AmmoTypes[i].maximumCapacity);
        }
    }

    public void Fire()
    {
        if(currentWeaponData.isAutomatic)
        {
            automaticRoutine = StartCoroutine(AutomaticRoutine());
        }
        else
        {
            BulletFire();
        }
        
    }

    public void endFIre()
    {
        if (automaticRoutine != null)
        {
            StopCoroutine(automaticRoutine);
        }
        
    }

    IEnumerator AutomaticRoutine()
    {
        while (ammoStore[currentWeaponData.ammoType] >0)
        {
            BulletFire();
            yield return new WaitUntil(() => { return (Time.time >= nextFireTime); }); // lamda function - anonymous method to look for a function which retunrs a bool
        }
        BulletFire();
    }

    void BulletFire()
    {
        if (recoiling)
        {
            if (Time.time >= nextFireTime)
            {
                recoiling = false;
            }
            else
            {
                return;
            }
        }

        //OUT OF AMMO
        if (ammoStore[currentWeaponData.ammoType] <= 0)
        {
            //play empty clip sound (out of ammo)
            print("clip is empty");
            return;
        }
        else
        {
            updateAmmo( -1 );
        }

        Instantiate(currentWeaponData.E_muzzleFlash, currentGun.Muzzle.position, currentGun.Muzzle.rotation, currentGun.Muzzle);

        //get bullet position correct (
        Vector3 bulletdir = ((viewCamera.transform.position + viewCamera.transform.forward * 1000) - currentGun.Muzzle.position).normalized;
        // this will get the direction of the bullet frm the line to get the rotation angle of the rifle
        bool didHit = false;

        RaycastHit hit; // store info from raycast
        if (Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward, out hit, Mathf.Infinity, hitMask))
        //shoot a ray from camera forward for infinity until it hits will return true or false
        {
            TakeDamageTest hitObj = hit.collider.GetComponent<TakeDamageTest>();
            if (hitObj != null)
            {
                hitObj.TakeDamage(currentWeaponData.ammoType.damage); //this using dependency injection
            }
            print(hit.collider.gameObject.name + "was hit at " + hit.point);
            Instantiate(currentWeaponData.E_hitEffect, hit.point, Quaternion.identity);
            bulletdir = (hit.point - currentGun.Muzzle.position).normalized;
        }

        Instantiate(currentWeaponData.E_bullet,
            currentGun.Muzzle.position,
            Quaternion.LookRotation(bulletdir)).GetComponent<BulletControl>().Initialise(didHit, hit.point);

        recoiling = true;
        nextFireTime = Time.time + currentWeaponData.fireRate;
    }

    void updateAmmo(int value)
    {
        ammoStore[currentWeaponData.ammoType] += value;
        print("Bullets: " + ammoStore[currentWeaponData.ammoType]);
        //TODO: update GUI

    }

}

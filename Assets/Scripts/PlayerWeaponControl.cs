using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponControl : MonoBehaviour
{
    // delegate to be a generic ru n the correct fire function based off the type of weapon
    EventTypes.VoidDel fire;
    
    public Camera viewCamera;
    public LayerMask hitMask;
    public GunData currentWeaponData;

    public List<GunData> guns = new List<GunData>();
    public Transform gunPivot; // where we will instantiate the guns
    public int currentGunIndex = 0;

    AreaGun areaGunData;
    ProjectileGun projectileGunData;

    public GunControl currentGun;

    public List<AmmoType> AmmoTypes = new List<AmmoType>();
    public Dictionary<AmmoType, int> ammoStore = new Dictionary<AmmoType, int>();
    
    bool recoiling = false;
    float nextFireTime = 0;

    Coroutine automaticRoutine;

    public AmmoCounterGUIControl ammoGUI;
    public ReticleControl reticleGUI;

    public EventTypes.Vector3Event broadcastShot;
    
    // Start is called before the first frame update
    void Start()
    {
        InitialiseAmmo();
        BuildWeapon(currentGunIndex);
        ammoGUI.InitialiseGUI(currentWeaponData, ammoStore[currentWeaponData.ammoType]);
        reticleGUI.ChamgeReticle(currentWeaponData.reticleSprite, currentWeaponData.reticleSize);
        SetGunFireFunction();
    }

    public void SwitchWeapons(int direction)
    {
        int gunCount = guns.Count - 1;
        int id = currentGunIndex + direction;
        //if(id <= gunCount && id >= 0) // if id is within list set curretn weapon to id
        //{
        //    currentGunIndex = id;
        //    print($"Scrollimg = {gunCount}");
        //}
        if(id > gunCount) // if out of bounds if its too large
        {
            id = 0;
            currentGunIndex = id;
            //print($"Scrollimg > {gunCount}");
        }
        else if(id < 0)// if out of bounds if its too low
        {
            id = gunCount;
            currentGunIndex = id;
            //print($"Scrollimg < {gunCount}");
        }
        currentGunIndex = id;

        ClearWeapon();
        BuildWeapon(currentGunIndex);
        ammoGUI.InitialiseGUI(currentWeaponData, ammoStore[currentWeaponData.ammoType]);
        reticleGUI.ChamgeReticle(currentWeaponData.reticleSprite, currentWeaponData.reticleSize);
        SetGunFireFunction();

    }

    void ClearWeapon()
    {
        Destroy(currentGun.gameObject);
        currentWeaponData = null;
        areaGunData = null;
        projectileGunData = null;
    }

    void BuildWeapon(int gunId)
    {
        currentWeaponData = guns[gunId];
        currentGun = Instantiate(currentWeaponData.preFab,
            gunPivot.TransformPoint(currentWeaponData.pivotOffset),
            gunPivot.rotation, 
            gunPivot).GetComponent<GunControl>(); //GetComponent.GunControl stores our muzzle
                                                    //instantiates it and assigns it to currentGun

        //THEN IMPLEMENT GUI CHANGES
    }

    void SetGunFireFunction()
    {
        if(currentWeaponData.GetType() == typeof(ProjectileGun))
        {
            projectileGunData = currentWeaponData as ProjectileGun;
            fire = ProjectileFire;
        }
        else if (currentWeaponData.GetType() == typeof(AreaGun))
        {
            areaGunData = currentWeaponData as AreaGun;
            fire = AreaGunFire;
        }
        else
        {
            fire = BulletFire;
        }
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
            fire();
            broadcastShot.Invoke(transform.position);
        }
        
    }

    public void endFire()
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
            fire();
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
            updateAmmo( -1, currentWeaponData.ammoType);
        }

        Instantiate(currentWeaponData.E_muzzleFlash, currentGun.Muzzle.position, currentGun.Muzzle.rotation, currentGun.Muzzle);

        //get bullet position correct (
        Vector3 bulletdir = ((viewCamera.transform.position + viewCamera.transform.forward * 1000) - currentGun.Muzzle.position).normalized;
        // this will get the direction of the bullet frm the line to get the rotation angle of the rifle
        bool didHit = false;

        RaycastHit hit; // store info from raycast
        if (FireRay(viewCamera.transform.forward, out hit))
        //shoot a ray from camera forward for infinity until it hits will return true or false
        {
            bool hitBody = false;
            TakeDamageTest hitObj = hit.collider.GetComponent<TakeDamageTest>();
            if (hitObj != null)
            {
                hitObj.TakeDamage(currentWeaponData.ammoType.damage); //this using dependency injection
                print("hit bullet");
                hitBody = true;
            }
            //print(hit.collider.gameObject.name + "was hit at " + hit.point);
            if (hitBody)
            {
                Instantiate(hitObj.E_bloodSplatter, hit.point, Quaternion.identity);
            }
            else
            {
                Instantiate(currentWeaponData.E_hitEffect, hit.point, Quaternion.identity);
            }
            
            bulletdir = (hit.point - currentGun.Muzzle.position).normalized;
            didHit = true;
        }

        Instantiate(currentWeaponData.E_bullet,
            currentGun.Muzzle.position,
            Quaternion.LookRotation(bulletdir)).GetComponent<BulletControl>().Initialise(didHit, hit.point);

        recoiling = true;
        nextFireTime = Time.time + currentWeaponData.fireRate;
    }

    void AreaGunFire()
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
            updateAmmo(-1, currentWeaponData.ammoType);
        }

        Instantiate(currentWeaponData.E_muzzleFlash, currentGun.Muzzle.position, currentGun.Muzzle.rotation, currentGun.Muzzle);

        

        for(int i = 0; i < areaGunData.shotCount; i++)
        {
            //get bullet position correct (
            //Vector3 bulletdir = ((viewCamera.transform.position + viewCamera.transform.forward * 1000) - currentGun.Muzzle.position).normalized;
            // this will get the direction of the bullet frm the line to get the rotation angle of the rifle
            bool didHit = false;
            RaycastHit hit; // store info from raycast
            
            Vector3 spread = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            if(spread.magnitude > 1)
            {
                spread.Normalize();
            }
            spread *= areaGunData.spreadRadius;
            Ray r = viewCamera.ScreenPointToRay(Input.mousePosition + spread);

            Vector3 bulletDir = ((viewCamera.transform.position + r.direction*200) - currentGun.Muzzle.position).normalized;

            if (Physics.Raycast(r, out hit, Mathf.Infinity, hitMask))
            //shoot a ray from camera forward for infinity until it hits will return true or false
            {
                bool hitBody = false;
                TakeDamageTest hitObj = hit.collider.GetComponent<TakeDamageTest>();
                if (hitObj != null)
                {
                    hitObj.TakeDamage(currentWeaponData.ammoType.damage); //this using dependency injection
                    hitBody = true;
                }

                if (hitBody)
                {
                    Instantiate(hitObj.E_bloodSplatter, hit.point, Quaternion.identity);
                }
                else
                {
                    Instantiate(currentWeaponData.E_hitEffect, hit.point, Quaternion.identity);
                }
                //bulletdir = (hit.point - currentGun.Muzzle.position).normalized;
                didHit = true;
            }

            if (didHit)
            {
                bulletDir = hit.point - currentGun.Muzzle.position.normalized; // set bullet directions properly
            }
            
            //vectors for bullet
            Instantiate(currentWeaponData.E_bullet,
            currentGun.Muzzle.position,
            Quaternion.LookRotation(bulletDir)).GetComponent<BulletControl>().Initialise(didHit, hit.point);
        }

        recoiling = true;
        nextFireTime = Time.time + currentWeaponData.fireRate;
    }

    void ProjectileFire()
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
            updateAmmo(-1, currentWeaponData.ammoType);
        }

        Instantiate(currentWeaponData.E_muzzleFlash, currentGun.Muzzle.position, currentGun.Muzzle.rotation, currentGun.Muzzle);

        Vector3 endPoint;
        RaycastHit hit;

        if(FireRay(viewCamera.transform.forward, out hit))
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = viewCamera.transform.position + (viewCamera.transform.forward * 1000);
        }

        // generate projectile direction
        Vector3 projectileDirection = (endPoint - currentGun.Muzzle.position).normalized;
        Instantiate(projectileGunData.projectile, currentGun.Muzzle.position, Quaternion.LookRotation(projectileDirection));

        recoiling = true;
        nextFireTime = Time.time + currentWeaponData.fireRate;
    }

    bool FireRay(Vector3 direction, out RaycastHit hitInfo)
    {
        return Physics.Raycast(viewCamera.transform.position, direction, out hitInfo, Mathf.Infinity, hitMask);
    }

    bool FireRay(Vector3 direction, out RaycastHit hitInfo, float distance)
    {
        return Physics.Raycast(viewCamera.transform.position, direction, out hitInfo, distance, hitMask);
    }
    //void updateAmmo(int value)
    //{
    //    //ammoStore[currentWeaponData.ammoType] += value;
    //    ammoStore[currentWeaponData.ammoType] = Mathf.Clamp(ammoStore[currentWeaponData.ammoType] 
    //        + value, 0, currentWeaponData.ammoType.maximumCapacity);
    //    //print("Bullets: " + ammoStore[currentWeaponData.ammoType]);

    //    //TODO: update GUI

    //}

    void updateAmmo(int value, AmmoType type) // overload method if ammo type is not matched
    {
        ammoStore[type] = Mathf.Clamp(ammoStore[type] + value, 0, 
            type.maximumCapacity);
        if(type == currentWeaponData.ammoType)
        {
            //update gui
            ammoGUI.UpdateAmmo(ammoStore[type]);
        }

    }

    public void AmmoPickUp(int value, AmmoType type)
    {
        updateAmmo(value, type);
    }

}

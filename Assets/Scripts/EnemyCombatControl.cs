using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatControl : MonoBehaviour
{    // delegate to be a generic ru n the correct fire function based off the type of weapon
    EventTypes.VoidVec3Del fire;

    private AudioManager audioManager;

    public LayerMask hitMask;
    public GunData currentWeaponData;

    public Transform gunPivot; // where we will instantiate the guns

    AreaGun areaGunData;
    ProjectileGun projectileGunData;

    public GunControl currentGun;
    public HurtBoxControl currentHurtBox;

    bool recoiling = false;
    float nextFireTime = 0;

    Coroutine automaticRoutine;

    [Header("Melee")]
    public EventTypes.MeleeAttackEvent meleeAttack;

    void Start()
    {
        audioManager = AudioManager.instance;
        BuildWeapon();
        SetGunFireFunction();
    }

    void BuildWeapon()
    {
        if(currentWeaponData.isMelee && currentWeaponData.preFab == null)
        {
            return;
        }
        currentGun = Instantiate(currentWeaponData.preFab,
            gunPivot.TransformPoint(currentWeaponData.pivotOffset),
            gunPivot.rotation,
            gunPivot).GetComponent<GunControl>(); //GetComponent.GunControl stores our muzzle
                                                  //instantiates it and assigns it to currentGun

        //THEN IMPLEMENT GUI CHANGES
    }

    void SetGunFireFunction()
    {
        if (currentWeaponData.GetType() == typeof(ProjectileGun))
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

    public void StartMeleeCast()
    {
        currentHurtBox.StartCast();
    }

    public void EndMeleeCast()
    {
        currentHurtBox.EndCast();
    }

    public void MeleAttack(int attackIndex)
    {
        if (currentWeaponData.isMelee && currentWeaponData.meleeAttacks.Count > 0
            && attackIndex < +currentWeaponData.meleeAttacks.Count)
        {
            // run the attack
            meleeAttack.Invoke(currentWeaponData.meleeAttacks[attackIndex]);
            //audioManager.PlaySound(audioManager.enemySlashSound);
        }
        else if (!currentWeaponData.isMelee)
        {
            print("weapon is not a melee weapon");
        }
        else if (currentWeaponData.isMelee && currentWeaponData.meleeAttacks.Count == 0)
        {
            print("Weapon is a melee weapon, but attacks listed");
        }
        else if (currentWeaponData.isMelee 
            && currentWeaponData.meleeAttacks.Count > 0 && attackIndex > currentWeaponData.meleeAttacks.Count)
        {
            print("Weapon is a melee weapon, and has attacks but attack index is too high");
        }

    }

    public void Fire(bool hit)
    {
        if (currentWeaponData.isAutomatic)
        {
            automaticRoutine = StartCoroutine(AutomaticRoutine());
        }
        else
        {
            fire(GetFireDirection(hit));
        }

    }

    public void EndFire()
    {
        if (automaticRoutine != null)
        {
            StopCoroutine(automaticRoutine);
        }
    }

    Vector3 GetFireDirection(bool hit)
    {
        Vector3 fireDirection = gunPivot.forward;
        
        if (!hit)
        {
            fireDirection 
                = gunPivot.TransformDirection(new Vector3(Random.Range(-1,1),
                Random.Range(-1, 1),
                Random.Range(-1,1)).normalized * 0.2f);
        }
        return fireDirection;
    }

    IEnumerator AutomaticRoutine()
    {
        while (true)
        {
            fire(gunPivot.forward);
            yield return new WaitUntil(() => { return (Time.time >= nextFireTime); }); // lamda function - anonymous method to look for a function which retunrs a bool
        }

        fire(gunPivot.forward);
    }

    void BulletFire(Vector3 direction)
    {
        if (currentWeaponData.ammoType == null) /// if no ammo type return ie if its melee
        {
            return;
        }
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

        Instantiate(currentWeaponData.E_muzzleFlash, currentGun.Muzzle.position, currentGun.Muzzle.rotation, currentGun.Muzzle);
        //audioManager.PlaySound(audioManager.enemyShotSound);
        //get bullet position correct (
        Vector3 bulletdir = ((gunPivot.transform.position + direction * 1000) - currentGun.Muzzle.position).normalized;
        // this will get the direction of the bullet frm the line to get the rotation angle of the rifle
        bool didHit = false;

        RaycastHit hit; // store info from raycast
        if (FireRay(direction, out hit))
        //shoot a ray from camera forward for infinity until it hits will return true or false
        {
            TakeDamageTest hitObj = hit.collider.GetComponent<TakeDamageTest>();
            if (hitObj != null)
            {
                hitObj.TakeDamage(currentWeaponData.ammoType.damage); //this using dependency injection
            }
            //print(hit.collider.gameObject.name + "was hit at " + hit.point);
            Instantiate(currentWeaponData.E_hitEffect, hit.point, Quaternion.identity);
            bulletdir = (hit.point - currentGun.Muzzle.position).normalized;
            didHit = true;
        }

        Instantiate(currentWeaponData.E_bullet,
            currentGun.Muzzle.position,
            Quaternion.LookRotation(bulletdir)).GetComponent<BulletControl>().Initialise(didHit, hit.point);

        recoiling = true;
        nextFireTime = Time.time + currentWeaponData.fireRate;
    }

    void AreaGunFire(Vector3 direction)
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

        Instantiate(currentWeaponData.E_muzzleFlash, currentGun.Muzzle.position, currentGun.Muzzle.rotation, currentGun.Muzzle);
        //audioManager.PlaySound(audioManager.enemyShotSound);


        for (int i = 0; i < areaGunData.shotCount; i++)
        {
            //get bullet position correct (
            //Vector3 bulletdir = ((viewCamera.transform.position + viewCamera.transform.forward * 1000) - currentGun.Muzzle.position).normalized;
            // this will get the direction of the bullet frm the line to get the rotation angle of the rifle
            bool didHit = false;
            RaycastHit hit; // store info from raycast

            Vector3 spread = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            if (spread.magnitude > 1)
            {
                spread.Normalize();
            }
            spread *= areaGunData.spreadRadius;
            Vector3 firePos = gunPivot.TransformPoint(spread);
            //Ray r = viewCamera.ScreenPointToRay(Input.mousePosition + spread);
                                //gunPivot.transform.position
            Vector3 bulletDir = ((firePos + gunPivot.forward * 200) - currentGun.Muzzle.position).normalized;

            if (Physics.Raycast(firePos, gunPivot.forward, out hit, Mathf.Infinity, hitMask))
            //shoot a ray from camera forward for infinity until it hits will return true or false
            {
                TakeDamageTest hitObj = hit.collider.GetComponent<TakeDamageTest>();
                if (hitObj != null)
                {
                    hitObj.TakeDamage(currentWeaponData.ammoType.damage); //this using dependency injection
                }

                Instantiate(currentWeaponData.E_hitEffect, hit.point, Quaternion.identity);
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

    void ProjectileFire(Vector3 direction)
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

        Instantiate(currentWeaponData.E_muzzleFlash, currentGun.Muzzle.position, currentGun.Muzzle.rotation, currentGun.Muzzle);
        //audioManager.PlaySound(audioManager.enemyShotSound);
        Vector3 endPoint;
        RaycastHit hit;

        if (FireRay(direction, out hit))
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = gunPivot.transform.position + (direction * 1000);
        }

        // generate projectile direction
        Vector3 projectileDirection = (endPoint - currentGun.Muzzle.position).normalized;
        Instantiate(projectileGunData.projectile, currentGun.Muzzle.position, Quaternion.LookRotation(projectileDirection));

        recoiling = true;
        nextFireTime = Time.time + currentWeaponData.fireRate;
    }

    bool FireRay(Vector3 direction, out RaycastHit hitInfo)
    {
        return Physics.Raycast(gunPivot.transform.position, direction, out hitInfo, Mathf.Infinity, hitMask);
    }

    bool FireRay(Vector3 direction, out RaycastHit hitInfo, float distance)
    {
        return Physics.Raycast(gunPivot.transform.position, direction, out hitInfo, distance, hitMask);
    }
}

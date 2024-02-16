using System.Collections;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A gun that fires physical projectile objects rather than hit scanning with ray casts
/// </summary>
[CreateAssetMenu()]
public class ProjectileGun : GunData
{
    [Header("Projectile Gun")]
    public GameObject projectile;
}

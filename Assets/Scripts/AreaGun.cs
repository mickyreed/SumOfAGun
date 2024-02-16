using System.Collections;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A gun that fires a number of ray casts in an area rather than a single projectile
/// </summary>
[CreateAssetMenu()]
public class AreaGun : GunData
{
    [Header("Area Gun")]
    [Tooltip("How many bullets ar fireed per shot of this weapon")]
    public int shotCount = 10;
    [Tooltip("How radius of the random spread of the shots of this weapon")]
    public float spreadRadius = 1f;
    [Tooltip("How optimum effective range of this weapon")]
    public float range = 10f;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class GunData : ScriptableObject
{
    [Tooltip("The prefab of the gun that gets loaded into the player's hands")]
    public GameObject preFab;

    [Tooltip("Is the gun single shot or Automatic fire")]
    public bool isAutomatic = false;

    [Tooltip("time between shots for this weapon")]
    public float fireRate = 0.1f;

    [Tooltip("type of this ammo used by this weapon")]
    public AmmoType ammoType;

    [Tooltip("Offset for spawning the gun into the players hands")]
    public Vector3 pivotOffset;

    [Header("GUI")]
    public Sprite reticleSprite;
    public Vector2 reticleSize;

    [Header("Effect")]
    [Tooltip("The projectile launched by the gun")]
    public GameObject E_bullet;

    [Tooltip("effect produced at muzzle location when gun is fired")]
    public GameObject E_muzzleFlash;
    [Tooltip("The effect created at point of bullet flash projectile impact")]
    public GameObject E_hitEffect;
    [Tooltip("The texture left behind after bullet impact")]
    public GameObject E_bulletMark; // impact effect

    [Header("Audio")]
    [Tooltip("The SFX for firing the gun")]
    public AudioClip A_fire;
    [Tooltip("The SFX for firing without ammo")]
    public AudioClip A_dryFire;
    [Tooltip("The SFX of projectile hitting the target")]
    public AudioClip A_impact;

}

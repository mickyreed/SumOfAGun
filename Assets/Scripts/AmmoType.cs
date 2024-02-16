using System.Collections;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]

public class AmmoType : ScriptableObject
{
    [Tooltip("maximum amount of this ammo that can be held by the player")]
    public int maximumCapacity = 50;

    [Tooltip("Damage delat per hit with this ammo type")]
    public int damage = 1;
}

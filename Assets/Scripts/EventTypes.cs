using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventTypes
{
    public delegate void VoidDel();
    public delegate void VoidVec3Del(Vector3 vector);
    public delegate void VoidBoolDel(bool value);

    [System.Serializable]

    public class IntEvent : UnityEvent<int>
    {

    }

    [System.Serializable]

    public class IntAmmoEvent : UnityEvent<int, AmmoType>
    {

    }
}

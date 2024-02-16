using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventTypes
{
    [System.Serializable]

    public class IntEvent : UnityEvent<int>
    {

    }
}

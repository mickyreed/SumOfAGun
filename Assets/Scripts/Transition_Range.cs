using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Transition_Range
{
    public float range = 10f;
    public FSM_Base stateToEnter;
    public bool overrrideLifetime = false;
    public bool playerMustBeTarget = true;
}

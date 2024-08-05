using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiisableOnStart : MonoBehaviour
{
    public GameObject[] objectsToDisable; // Array of GameObjects to disable on start

    void Start()
    {
        // Loop through each GameObject in the array and disable them
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }
}

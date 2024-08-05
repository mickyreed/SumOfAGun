using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu; // Reference to the settings menu GameObject
    public bool isActive;

    void Start()
    {
        // Disable the settings menu when the scene starts
        settingsMenu.SetActive(false);
        isActive = false;
    }

    // Method to toggle the visibility of the settings menu
    public void ToggleSettingsMenu()
    {
        if (!isActive)
        {
            settingsMenu.SetActive(!settingsMenu.activeSelf);
            //settingsMenu.SetActive(true);
        }
        else
        {
            settingsMenu.SetActive(false);
        }
        
    }
}

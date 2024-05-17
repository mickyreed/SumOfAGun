using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    // Quit Game Method
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControl : MonoBehaviour
{
    // unlock cursor when  we first start so its not locked from a pause state when we have returned from in game
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor
    }
}

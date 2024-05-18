using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PauseControl : MonoBehaviour
{
    public static PauseControl instance;
    public EventTypes.VoidBoolDel pause;

    public GameObject pauseMenuUI; // Reference to the Pause Menu UI
    //public MonoBehaviour playerMovementScript; // Reference to the player's movement script
    //public MonoBehaviour playerShootingScript; // Reference to the player's shooting script

    bool paused = false;
    bool gameOver = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;
        if(instance != null && instance != this) // if an instance exists thats not this object,
        {
            Destroy(instance); // destroy it
        }
        PauseControl.instance = this; // set ourselves to be the instance
        DontDestroyOnLoad(gameObject); /// prevents object from being destroyed when next scene is entered
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused; // if its true set to false, if its true set to false
            if (paused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
            
            pause?.Invoke(paused);
        }
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true); // Show the pause menu
        //playerMovementScript.enabled = false; // Disable player movement
        //playerShootingScript.enabled = false; // Disable player shooting
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true; // Show cursor
    }

    public void UnPause()
    {
        paused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false); // Hide the pause menu
        //playerMovementScript.enabled = true; // Enable player movement
        //playerShootingScript.enabled = true; // Enable player shooting
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false; // Hide cursor
        pause?.Invoke(paused);
    }

    public void GameOver()
    {
        gameOver = true;
    }

    public void QuitGame()
    {
        // Code to quit the game
        Application.Quit();
    }
}

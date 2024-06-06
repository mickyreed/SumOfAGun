using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class GameOverControl : MonoBehaviour
{
    public static GameOverControl instance;
    public EventTypes.VoidBoolDel pause;
    public GameObject GameOverMenuUI; // Reference to the Pause Menu UI

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
        GameOverControl.instance = this; // set ourselves to be the instance
        DontDestroyOnLoad(gameObject); /// prevents object from being destroyed when next scene is entered
    }

    // Update is called once per frame
    void Update()
    {

        //GameOver();

        if (gameOver)
        {
            Pause();
            //pause?.Invoke(paused);
            return;
        }
        
    }

    public void TriggerGameOver()
    {
        GameOver();
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        GameOverMenuUI.SetActive(true); // Show the pause menu
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true; // Show cursor

    }

    public void UnPause()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false; // Hide cursor
        Time.timeScale = 1f;
        GameOverMenuUI.SetActive(false);
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

    public void ReturnToMainMenu()
    {
        // Code to quit the game
        UnPause();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
}

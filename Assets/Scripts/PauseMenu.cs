using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject player;
    private PlayerMovement PlayerMovement;
    private PlayerInput PlayerInput;
    //public MonoBehaviour playerMovementScript; // Reference to the player's movement script
    //public MonoBehaviour playerShootingScript; // Reference to the player's shooting script

    void Start()
    {
        PlayerMovement = player.GetComponent<PlayerMovement>();
        if (PlayerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on the player GameObject.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Debug.Log("Unpausing Game...");
        PlayerMovement.enabled = true;
        PlayerInput.enabled = true;
        //playerShootingScript.enabled = true; // Disable player shooting
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Debug.Log("Pausing Game...");
        PlayerMovement.enabled = false;
        PlayerInput.enabled = false;
        //playerShootingScript.enabled = false; // Disable player shooting
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu...");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
    }
}

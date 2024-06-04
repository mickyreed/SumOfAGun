using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public static GameManager Instance;
    private AudioManager audioManager;
    private void Awake()
    {
        //Instance = this;
        //SceneManager.LoadSceneAsync(SceneIndexes.TitleScreen, LoadSceneMode.Additive);
    }

    private void Start()
    {
        //Instance = this;
        //SceneManager.LoadSceneAsync(SceneIndexes.TitleScreen, LoadSceneMode.Additive);
        audioManager = AudioManager.instance;
        audioManager.PlaySound(audioManager.beamDownSound);
        audioManager.PlayMusic(audioManager.backgroundMusic);
    }

    // Update is called once per frame
    public void LoadGame()
    {
        //SceneManager.UnloadSceneAsync((int)SceneIndexes.TitleScreen);
        //SceneManager.LoadSceneAsync((int)SceneIndexes.Level1, LoadSceneMode.Additive);

    }

    public void PauseGame()
    {
        // Pause the game logic
        audioManager.StopMusic();
    }

    public void ExitGame()
    {
        // Save progress and exit the game
        audioManager.StopMusic();
        GameOverControl.instance.TriggerGameOver();
    }
}

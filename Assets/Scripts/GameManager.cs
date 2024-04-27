using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
        //SceneManager.LoadSceneAsync(SceneIndexes.TitleScreen, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    public void LoadGame()
    {
        //SceneManager.UnloadSceneAsync((int)SceneIndexes.TitleScreen);
        //SceneManager.LoadSceneAsync((int)SceneIndexes.Level1, LoadSceneMode.Additive);

    }
}

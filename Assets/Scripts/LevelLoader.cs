using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public CanvasGroup crossFadeCanvasGroup;
    public float fadeDuration = 1f; // Adjust this to control the duration of the fade

    private bool isTransitioning = false;

    public string nextSceneName; // Index of the scene to load next
    public string sceneToLoad; // Field to store the scene index to load

    void Start()
    {
        // Start the cross fade animation
        PlayFadeAnimation("CrossFade_Start");
    }

    void Update()
    {
        Debug.Log("Update function is called "+ nextSceneName + sceneToLoad); // Add this line for debugging
        // Check for user input to trigger transition
        if ((!isTransitioning) && Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            Debug.Log("**************** Inside recieved input **********************");
            isTransitioning = true;
            // Start the cross fade animation
            PlayFadeAnimation("CrossFade_End");
            // Load the level after the fade out
            //Invoke(nameof(LoadNextScene), fadeDuration);
            SceneManager.LoadScene(1);
        }

    }

    void PlayFadeAnimation(string animationName)
    {
        Debug.Log("Play Fade Animation is called");
        // Play the specified animation
        //crossFadeCanvasGroup.GetComponent<Animation>().Play(animationName);
    }

    //void LoadNextScene()
    //{
    //    Load the level_1 scene
    //    SceneManager.LoadScene("Level_1");
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}

    void LoadNextScene()
    {
        Debug.Log("Load Next Scene is called");
        //Load the next scene using the provided scene name
        LoadSceneByName(nextSceneName);
    }

    // Function to allow loading of any scene by name
    public void LoadSceneByName(string sceneName)
    {
        if (!isTransitioning)
        {
            Debug.Log("Load Scene by Name is called");
            isTransitioning = true;
            // Start the cross fade animation
            PlayFadeAnimation("CrossFade_End");
            // Store the scene name to load
            sceneToLoad = sceneName;
            // Load the specified scene after the fade out
            Invoke("LoadSceneByNameDelayed", fadeDuration);
        }
    }

    void LoadSceneByNameDelayed()
    {
        Debug.Log("Load Scene By NameDelayed");
        // Load the scene with the specified name
        //SceneManager.LoadScene(sceneToLoad);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(sceneToLoad);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Loader : MonoBehaviour
{
    public float fadeDuration = 1f; // Adjust this to control the duration of the fade

    void Start()
    {
        // Start the cross fade animation
        Invoke("LoadSceneByNameDelayed", fadeDuration);
        PlayFadeAnimation("CrossFade_Start");
    }

    void PlayFadeAnimation(string animationName)
    {
        Debug.Log("Play Fade Animation is called");
        // Play the specified animation
        //crossFadeCanvasGroup.GetComponent<Animation>().Play(animationName);
        Invoke("LoadSceneByNameDelayed", fadeDuration);
    }

    void LoadSceneByNameDelayed()
    {
        Debug.Log("Loading fadescreen Delayed");

    }

}
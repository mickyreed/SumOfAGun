using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PauseControl : MonoBehaviour
{
    public static PauseControl instance;
    public EventTypes.VoidBoolDel pause;
    bool paused = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;
        if(instance != null && instance != this) // if an instance exists thats not this object,
        {
            Destroy(instance); // destoy it
        }
        PauseControl.instance = this; // set ourselves to be the instance
        DontDestroyOnLoad(gameObject); /// prevents object from being destroyed when next scene is entered
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused; // if its true set to false, if its true set to false
            if (paused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
            //if(paused != null) // if the event has subscribing functions
            //{
            pause?.Invoke(paused);
            //}
        }
    }

    public void UnPause()
    {
        paused = false;
        Time.timeScale = 1f;
        pause?.Invoke(paused);
    }

}

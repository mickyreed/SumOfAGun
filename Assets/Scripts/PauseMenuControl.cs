using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PauseControl.instance.pause += Pause;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Pause(bool paused)
    {
        gameObject.SetActive(paused);
        // (paused)
        //{
        //    gameObject.SetActive(true);
        //}
        //else
        //{
        //    gameObject.SetActive(false);
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPlate : MonoBehaviour
{
    public GameObject GameCompleteMenu;
    public Animator plateAnim;
    public GameManager gameManager;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GameCompleteMenu.SetActive(true);
            gameManager.ExitGame();

        }
    }
}

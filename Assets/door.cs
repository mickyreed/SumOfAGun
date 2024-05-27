using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public Animator doorAnim;
    public GameObject AreaToSpawn;

    public bool requiresKey;
    public bool reqRed, reqBlue, reqGreen;
    
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (requiresKey)
            {
                if(reqRed && other.GetComponent<PlayerInventory>().hasRedKey)
                {
                    //
                    doorAnim.SetTrigger("OpenDoor");
                    AreaToSpawn.SetActive(true);
                }
                if (reqBlue && other.GetComponent<PlayerInventory>().hasBlueKey)
                {
                    //
                    doorAnim.SetTrigger("OpenDoor");
                    AreaToSpawn.SetActive(true);
                }
                if (reqGreen && other.GetComponent<PlayerInventory>().hasGreenKey)
                {
                    //
                    doorAnim.SetTrigger("OpenDoor");
                    AreaToSpawn.SetActive(true);
                }
            }
            else
            {
                //open door
                doorAnim.SetTrigger("OpenDoor");
                AreaToSpawn.SetActive(true);
            }

        }
    }


}

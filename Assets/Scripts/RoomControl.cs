using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
    bool playerInRoom = false;
    GameObject playerRef = null;
    HashSet<FSM_Brain> aisInRoom = new HashSet<FSM_Brain>();
    Vector3 lastPlayerPos;

    public void PropagateSound(Vector3 soundPos)
    {
        lastPlayerPos = soundPos;
        foreach(FSM_Brain ai in aisInRoom)
        {
            ai.HearSound(soundPos); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AI")
        {
            FSM_Brain newAI = other.GetComponent<FSM_Brain>();
            if(!aisInRoom.Contains(newAI))
            {
                aisInRoom.Add(newAI);
            } 
        }

        if(other.tag == "Player" && !playerInRoom)
        {
            playerInRoom = true;
            playerRef = other.gameObject;
            
            //  pass the room reference to the player broadcaster
            playerRef.GetComponent<PlayerBroadcaster>().RecieveRoom(this);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "AI")
        {
            FSM_Brain newAI = other.GetComponent<FSM_Brain>();
            if (!aisInRoom.Contains(newAI))
            {
                aisInRoom.Remove(newAI);
            }

            if (other.tag == "Player")
            {
                playerInRoom = false;
                playerRef = null;
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBroadcaster : MonoBehaviour
{
    RoomControl currentRoom;

    public void RecieveRoom(RoomControl newRoom)
    {
        currentRoom = newRoom;
    }

    public void BroadCastSound()
    {
        if (currentRoom != null)
        {
            currentRoom.PropagateSound(transform.position);
        }
    }
}

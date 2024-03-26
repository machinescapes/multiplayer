using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomIteambutton : MonoBehaviour
{
    public string RoomName;

    public void OnButtonPressed()
    {
        RoomList.instance.JoinRoomByName(RoomName);
        Debug.Log("Joining room: " + RoomName);

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    GameObject LevelManager;
    GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("Canvas");
        LevelManager = GameObject.Find("LevelManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            string roomName = GetComponent<Room>().GetRoomName();
            RoomType roomType = GetComponent<Room>().GetRoomType();
            int roomInLevel = Convert.ToInt32(GetComponent<Room>().getRoomID()[0]);
            UI.GetComponent<MsgDisplayer>().SetPlaceName(roomName, roomType);
            LevelManager.GetComponent<LevelManager>().SetPlayerInLevel(roomInLevel);
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonValidator : MonoBehaviour
{
    private Room currentRoomInstance;

    // Start is called before the first frame update
    void Awake()
    {
        currentRoomInstance = GetComponentInParent<Room>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Room otherRoom;
        if (other.TryGetComponent<Room>(out otherRoom))
        {
            if(otherRoom != currentRoomInstance)
            {
                Debug.Log(currentRoomInstance.name + "overlaps with" + otherRoom.name);
                GameObject.Find("RoomManager").GetComponent<RoomManager>().overlapDetect();
            }
        }
    }

    public bool isCollidingWithOtherRooms(DungeonValidator other)
    {
        if(currentRoomInstance != other.GetCurrentRoomInstance())
        {
            return true;
        }
        return false;
    }

    public Room GetCurrentRoomInstance()
    {
        return currentRoomInstance;
    }
}

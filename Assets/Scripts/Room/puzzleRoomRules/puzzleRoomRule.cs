using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class puzzleRoomRule : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected bool isSolved;
    [SerializeField] protected bool playerEnter;
    GameObject Door1;
    GameObject Door2;
    protected void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            playerEnter = true;

    }
    public bool GetPuzzleIsSolved() {
        return this.isSolved;
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerEnter = false;
            if (isSolved)
                GameObject.Find("Canvas").GetComponent<MsgDisplayer>().ClearSecondObjective();
        }

    }
    protected void SetDoorLock()
    {
        Transform exit1 = GetComponent<Room>().GetParentExit();
        Transform exit2 = null;
        if (GetComponent<Room>().GetRoomType() != RoomType.FINAL_PUZZLE)
            exit2 = GetComponent<Room>().getExitConnectingTo().First().Key;
        if (exit1 && exit2)
        {
            Door1 = exit1.GetComponent<RoomGenerator>().GetDoorObject();
            Door2 = exit2.GetComponent<RoomGenerator>().GetDoorObject();
            if (Door1 && Door2)
            {
                if (playerEnter && !isSolved)
                {
                    Door1.GetComponent<DoorBehavior>().SetDoorLock(true, "SOLVE THE PUZZLE");
                    Door2.GetComponent<DoorBehavior>().SetDoorLock(true, "SOLVE THE PUZZLE");
                }
                else if(!playerEnter || isSolved)
                {
                    //handle the situation that two puzzle room connect each other witch may cuase the conneting door lock conflict.
                    //Now the connecting door lock will follow the previous puzzle room solve status.If previous room puzzle not solve ,the connecting door between two will remain locked
                    GameObject previousRoom = exit1.parent.gameObject;
                    if (previousRoom.GetComponent<Room>().GetRoomType() == RoomType.PUZZLE)
                    {
                       bool previousRoomPuzzleSolved =  previousRoom.GetComponent<puzzleRoomRule>().GetPuzzleIsSolved();
                       if(previousRoomPuzzleSolved)
                            Door1.GetComponent<DoorBehavior>().SetDoorLock(false, "");
                    }
                    else
                    {
                        Door1.GetComponent<DoorBehavior>().SetDoorLock(false, "");
                    }
                    Door2.GetComponent<DoorBehavior>().SetDoorLock(false, "");
                }
            }
           
        }
        
    }
}

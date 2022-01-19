using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomBehavior: MonoBehaviour
{
    GameObject UI;
    [SerializeField] GameObject showCamera;
    [SerializeField] GameObject gravityPuzzleRoom;
    [SerializeField] GameObject moveStatueRoom;
    [SerializeField] GameObject gravityPuzzleLight;
    [SerializeField] GameObject moveStatueLight;
    [SerializeField] private GameObject outterButton;
    [SerializeField] private bool gravityPuzzleSolved;
    [SerializeField] private bool moveStatueSolved;
    [SerializeField] private bool gravitypuzzleChecked;
    [SerializeField] private bool moveStatueChecked;
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("Canvas");
        gravityPuzzleSolved = false ;
        moveStatueSolved = false;
        gravitypuzzleChecked = false;
        moveStatueChecked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gravityPuzzleRoom && moveStatueRoom)
        {
            if (gravityPuzzleRoom.GetComponent<puzzleRoomGravityPuzzleRule>().GetPuzzleIsSolved())
            {
                gravityPuzzleSolved = true;
                //ugly implement of show camera
                if (!gravitypuzzleChecked)
                {
                    showCamera.SetActive(true);
                    UI.GetComponent<MsgDisplayer>().ClearSecondObjective();
                    Invoke("TurnOffCamera", 3);
                    gravitypuzzleChecked = true;
                }
            }
            else
            {
                gravitypuzzleChecked = false;
            }
            if (moveStatueRoom.GetComponent<puzzleRoomMovestatueRule>().GetPuzzleIsSolved())
            {
                moveStatueSolved = true;
                //ugly implement of show camera
                if (!moveStatueChecked)
                {
                    showCamera.SetActive(true);
                    UI.GetComponent<MsgDisplayer>().ClearSecondObjective();
                    Invoke("TurnOffCamera", 3);
                    moveStatueChecked = true;
                }
            }
            else
            {
                moveStatueChecked = false;
            }
            if (gravityPuzzleSolved && moveStatueSolved)
            {
                outterButton.GetComponent<ElevatorButtonBehavior>().SetButtonActive();
            }
        }
        gravityPuzzleLight.transform.GetChild(1).GetComponent<EndRoomLightShining>().SetLightColor(gravityPuzzleSolved);
        moveStatueLight.transform.GetChild(1).GetComponent<EndRoomLightShining>().SetLightColor(moveStatueSolved);
    }
    public void TurnOffCamera()
    {
        showCamera.SetActive(false);
    }
    public void GetTwoPuzzleRooms()
    {
        gravityPuzzleRoom = GameObject.Find("finalpuzzleroom-gavitymazz(Clone)");
        moveStatueRoom = GameObject.Find("finalpuzzleroom-movestatue(Clone)");
    }
}

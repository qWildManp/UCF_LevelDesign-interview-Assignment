using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleRoomGravityPuzzleRule : puzzleRoomRule
{
    GameObject UI;
    [SerializeField] GameObject gravityPuzzle;
    private GravityPuzzleBehavior puzzleBehavior;
    private bool hasShowObjective;
    private void Start()
    {
        hasShowObjective = false;
        UI = GameObject.Find("Canvas");
        puzzleBehavior = gravityPuzzle.GetComponent<GravityPuzzleBehavior>();
        isSolved = false;
    }
    private void Update()
    {
        if (puzzleBehavior.GetSolved())
        {
            isSolved = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasShowObjective)
        {
            UI.GetComponent<MsgDisplayer>().SetSecondObjective("Solve the gravity puzzle");
            hasShowObjective = true;
        }
        
    }
}

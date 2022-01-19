using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO
public class GravityPuzzleBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject UI;
    [SerializeField] private Transform ballRespawnPoint;
    [SerializeField] private GameObject gameCamera;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject puzzlePlateform;
    private bool Interacting;
    private bool playerEnter;
    private bool solve;
    private Vector3 RespawnPos;
    private gravityPuzzleGoalTrigger goalTrigger;
    void Start()
    {
        UI = GameObject.Find("Canvas");
        RespawnPos =new Vector3 (ballRespawnPoint.position.x, ballRespawnPoint.position.y + 3f, ballRespawnPoint.position.z);
        Interacting = false;
        solve = false;
        gameCamera.SetActive(false);
        this.puzzlePlateform.GetComponent<DragRotate>().enabled = false;
        goalTrigger = goal.GetComponent<gravityPuzzleGoalTrigger>();
        ResetBallPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (solve)
        {
            return;
        }
        if (playerEnter)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!Interacting)
                {
                    SetInteract();
                    UI.GetComponent<MsgDisplayer>().SetHint("1.Control the panel to makes the ball reach the end \n2.Press R to reset the puzzle");
                    GetComponent<BoxCollider>().enabled = false;
                    Interacting = true;
                }
                else
                {
                    ResetInteract();
                    UI.GetComponent<MsgDisplayer>().ClearHint();
                    GetComponent<BoxCollider>().enabled = true;
                    Interacting = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.R)&&Interacting)
            {
                ResetPos();
            }
            if (goalTrigger.goal)
            {
                ResetInteract();
                UI.GetComponent<MsgDisplayer>().ClearHint();
                solve = true;
            }
        }
       
    }
    public bool GetSolved()
    {
        return this.solve;
    }
    private void SetInteract()
    {
        GameObject player = GameObject.Find("Player");
        Transform playerCamera = player.transform.GetChild(0);
        Transform playerController = player.transform.GetChild(2);
        playerController.GetComponent<CharacterController>().enabled = false;
        playerCamera.gameObject.SetActive(false);
        gameCamera.SetActive(true);
        puzzlePlateform.GetComponent<DragRotate>().enabled=true;
    }
    private void ResetInteract()
    {
        GameObject player = GameObject.Find("Player");
        Transform playerCamera = player.transform.GetChild(0);
        Transform playerController = player.transform.GetChild(2);
        playerController.GetComponent<CharacterController>().enabled = true;
        playerCamera.gameObject.SetActive(true);
        gameCamera.SetActive(false);
        puzzlePlateform.GetComponent<DragRotate>().enabled = false ;
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            playerEnter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerEnter = false;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
    public void ResetBallPos()
    {
        this.ball.transform.position = RespawnPos;
    }
    public void ResetPos()
    {
        this.puzzlePlateform.GetComponent<DragRotate>().ResetRotation();
        ResetBallPos();

    }
}

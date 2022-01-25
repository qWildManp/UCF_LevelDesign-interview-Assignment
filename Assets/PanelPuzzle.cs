using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPuzzle : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Buttons;
    public List<GameObject> PlayerPressOrder;
    public GameObject TriggerDoor;
    public List<AudioClip> Clips;
    public int playerOrder;
    public int playerPressNum;
    public bool isSolve;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(playerPressNum > 4&&!isSolve)
        {
            playerPressNum = 0;
            foreach (GameObject btn in Buttons)
            {
                if (btn.GetComponent<Renderer>().material.color == Color.red)
                {
                    ReSetAllButton();
                    PlayerPressOrder.Clear();
                    GetComponent<AudioSource>().clip = Clips[1];
                    GetComponent<AudioSource>().Play();
                    return;
                }
            }
            isSolve = true;
            SovlePuzzle();
        }
    }

    public void InitializePanelAnswer()
    {
        Buttons[0].GetComponent<PuzzleButton>().Order = 1; 
        Buttons[1].GetComponent<PuzzleButton>().Order = 3; 
        Buttons[2].GetComponent<PuzzleButton>().Order = 2; 
        Buttons[3].GetComponent<PuzzleButton>().Order = 4; 
    }


    private void SovlePuzzle()
    {
        TriggerDoor.GetComponent<DoorBehavior>().PuzzleSolvedDoor();
        GetComponent<AudioSource>().clip = Clips[0];
        GetComponent<AudioSource>().Play();
    }

    public bool CheckAns(GameObject button)
    {
        playerPressNum += 1;
        PuzzleButton pzButton = button.GetComponent<PuzzleButton>();
        PlayerPressOrder.Add(button);
        int idx = PlayerPressOrder.IndexOf(button);
        if (idx+1 == pzButton.Order)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReSetAllButton()
    {
        foreach (GameObject btn in Buttons)
        {
            btn.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}

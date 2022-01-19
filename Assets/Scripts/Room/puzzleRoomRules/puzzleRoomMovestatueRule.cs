using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleRoomMovestatueRule : puzzleRoomRule
{
    // Start is called before the first frame update
    GameObject UI;
    [SerializeField]private List<GameObject> Sets;
    [SerializeField]private List<GameObject> answerList;
    [SerializeField]private List<Transform> clueSpawnPoint;
    [SerializeField]private GameObject cluePrefab;
    [SerializeField]private bool needLock;
    [SerializeField]private bool randomAns;
    [SerializeField]private bool isFinalPuzzle;
    private GameObject clue;
    private int correctNum;
    private bool hasShowObjective;
    void Start()
    {
        hasShowObjective = false;
        UI = GameObject.Find("Canvas");
        correctNum = 0;
        isSolved =  false;
        playerEnter = false;
        if (randomAns && answerList.Count > 0)
        {
            List<GameObject> ansList = answerList;
            foreach (GameObject set in Sets)
            {
                puzzleSetRule setRule = set.GetComponent<puzzleSetRule>();
                int rndIndex = Random.Range(0, ansList.Count);
                GameObject rndAnswer = ansList[rndIndex];
                setRule.SetAnswer(rndAnswer);
                ansList.Remove(rndAnswer);
            }
            GenerateClue();
            SetClueMessage();
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(needLock)
            SetDoorLock();
        correctNum = 0;
        foreach(GameObject set in Sets)
        {
            puzzleSetRule setRule = set.GetComponent<puzzleSetRule>();
            if (setRule.CheckSetCorrect() == true)
            {
                correctNum += 1;
            }

        }
        if (correctNum == Sets.Count)
            isSolved = true;
        else
            isSolved = false;
    }

    private void GenerateClue()
    {
        if(clueSpawnPoint.Count > 0 && cluePrefab)
        {
            int rnd_index = Random.Range(0, clueSpawnPoint.Count);
            Transform rnd_spawnPoint = clueSpawnPoint[rnd_index];
            this.clue = Instantiate(cluePrefab);
            clue.GetComponent<RoomItem>().SetSpawnAt(rnd_spawnPoint);
        }
    }
    private void SetClueMessage()
    {
        string msg = "";
             string ans_1= Sets[0].GetComponent<puzzleSetRule>().GetAnswer().name;
             string ans_2= Sets[1].GetComponent<puzzleSetRule>().GetAnswer().name;
             string ans_3= Sets[2].GetComponent<puzzleSetRule>().GetAnswer().name;
        msg = "This is a dirty notebook. It seems to say how the statues are placed: \n" 
            + "<color=red>Left</color> shows the way to the door\n"
            + "<color=red>" + ans_1 + "</color> is on the left; \n"
            + "<color=red>" + ans_2 +"</color> is the middle one; \n"
            + "and finally ... the <color=red>" + ans_3 +"</color> is on the right;" ;
            
            clue.GetComponent<RoomItem>().SetItemDescribtion(msg);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && this.isFinalPuzzle &&!hasShowObjective)
        {
            UI.GetComponent<MsgDisplayer>().SetSecondObjective("Try to find out the correct position of statues");
            hasShowObjective = true;
        }
    }
}

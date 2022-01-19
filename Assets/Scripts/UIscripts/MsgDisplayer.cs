using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class MsgDisplayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject Message;
    private Transform Messageblank;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject PlaceNameblank;
    [SerializeField] private int msgCountdown;
    [SerializeField] private GameObject MainGameObjectives;
    [SerializeField] private GameObject SecondGameObjectives;
    [SerializeField] private GameObject Hintblank;
    
    private float msgCurrentCountDown;
    void Start()
    {
        msgCurrentCountDown = msgCountdown;
        Messageblank = Message.transform.Find("Msg_text");
    }

    // Update is called once per frame
    void Update()
    {
        if(GetMessage() != "")
        {
            Message.SetActive(true);
            msgCurrentCountDown -= Time.deltaTime;
            if (msgCurrentCountDown <= 0)
            {
                ClearMessage();
                Message.SetActive(false);
                msgCurrentCountDown = msgCountdown;
            }
        }
        else
        {
            msgCurrentCountDown = msgCountdown;
        }
       
    }
    public void SetPlaceName(string placename, RoomType roomtype)
    {
        Text temp = PlaceNameblank.GetComponent<Text>();
        temp.text = placename;
        switch (roomtype)
        {
            case RoomType.DANGER:
                temp.color = Color.red;
                break;
            case RoomType.PUZZLE:
                temp.color = Color.blue;
                break;
            case RoomType.FINAL_PUZZLE:
                temp.color = Color.blue;
                break;
            case RoomType.END:
                temp.color = Color.cyan;
                break;
            default:
                temp.color = Color.green;
                break;
        }
    }
    public void SetMessage(string msg)
    {
        Messageblank.GetComponent<Text>().text = msg;
    }
    public string GetMessage()
    {
        return Messageblank.GetComponent<Text>().text;
    }
    public void ClearMessage()
    {
        Messageblank.GetComponent<Text>().text = "";
    }
    public void SetMainObjective(string objective_text)//set main game objective
    {
        string previousObjective = MainGameObjectives.GetComponent<Text>().text;
        string modify_text = StrikeThrough(previousObjective);
        MainGameObjectives.GetComponent<Text>().text = modify_text;
        MainGameObjectives.GetComponent<Text>().DOFade(0, 3);
        StartCoroutine(SetNewGameObjectiveText(objective_text));
    }
    IEnumerator SetNewGameObjectiveText(string new_text)//set new game objetive text
    {
        yield return new WaitForSeconds(3.0f);
        MainGameObjectives.GetComponent<Text>().text = "";
        MainGameObjectives.GetComponent<Text>().DOFade(255, 1);
        MainGameObjectives.GetComponent<Text>().DOText(new_text,3);
    }
    public void SetSecondObjective(string objective_text)//set the secondary game objective
    {
        SecondGameObjectives.GetComponent<Text>().text = "";
        SecondGameObjectives.GetComponent<Text>().DOFade(255, 1);
        SecondGameObjectives.GetComponent<Text>().DOText(objective_text, 3);
    }
    public void ClearSecondObjective()//clear the secondary game objective
    {
        string previousObjective = SecondGameObjectives.GetComponent<Text>().text;
        string modify_text = StrikeThrough(previousObjective);
        SecondGameObjectives.GetComponent<Text>().text = modify_text;
        SecondGameObjectives.GetComponent<Text>().DOFade(0, 3);
    }

    public string StrikeThrough(string s)
    {
        string strikethrough = "";
        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }
        return strikethrough;
    }

    public void SetHint(string msg)
    {
        Hintblank.GetComponent<Text>().text = msg;
    }
    public void ClearHint()
    {
        Hintblank.GetComponent<Text>().text = "";
    }

    public void ActivePlayerDiedUI()
    {
        gameOverUI.SetActive(true);
        gameOverUI.transform.Find("GameOverText").GetComponent<TMP_Text>().text = "You Died";
    }
    public void ActivePlayerFinishUI()
    {
        gameOverUI.SetActive(true);
        gameOverUI.transform.Find("GameOverText").GetComponent<TMP_Text>().text = "To be continue ...";
    }
}

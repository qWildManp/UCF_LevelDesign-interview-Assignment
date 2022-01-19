using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleSetRule : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Answer;
    [SerializeField] private bool isCorrect;
    void Start()
    {
        isCorrect = false;
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        if (other.name == Answer.name)
            isCorrect = true;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == Answer.name)
            isCorrect = false;

    }
    public bool CheckSetCorrect()
    {
        return this.isCorrect;
    }
    public void SetAnswer(GameObject answer)
    {
        this.Answer = answer;
    }
    public GameObject GetAnswer()
    {
        return this.Answer;
    }
}

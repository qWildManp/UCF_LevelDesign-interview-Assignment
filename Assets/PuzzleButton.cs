using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    // Start is called before the first frame update
    public int Order;
    public bool correct;
    public GameObject Panel;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressButton()
    {
        GetComponent<AudioSource>().Play();
        correct =  Panel.GetComponent<PanelPuzzle>().CheckAns(gameObject);
        if (correct)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}

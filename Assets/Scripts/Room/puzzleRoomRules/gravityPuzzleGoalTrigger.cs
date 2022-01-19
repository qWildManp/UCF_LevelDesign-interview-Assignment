using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityPuzzleGoalTrigger : MonoBehaviour
{
    public bool goal;
    // Start is called before the first frame update
    private void Start()
    {
        goal = false;
    }
    private void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.name == "ball")
        {
            goal = true;
        }   
    }
}

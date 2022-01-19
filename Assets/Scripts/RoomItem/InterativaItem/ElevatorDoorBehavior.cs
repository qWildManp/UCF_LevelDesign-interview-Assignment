using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ElevatorDoorBehavior : MonoBehaviour
{
    [SerializeField] GameObject outterButton;
    [SerializeField] GameObject innerButton;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (outterButton&&innerButton)
        {
            if (innerButton.GetComponent<ElevatorButtonBehavior>().GetButtonPress())
                animator.SetBool("open", false);
            else if(outterButton.GetComponent<ElevatorButtonBehavior>().GetButtonPress())
                animator.SetBool("open", true);
            else
                animator.SetBool("open", false);
        }
        
    }
}

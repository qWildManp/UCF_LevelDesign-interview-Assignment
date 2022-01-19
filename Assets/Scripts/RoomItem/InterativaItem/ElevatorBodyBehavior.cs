using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class ElevatorBodyBehavior : MonoBehaviour
{
    GameObject UI;
    [SerializeField] GameObject innerButton;
    [SerializeField] GameObject elevatorDoor;
    private Animator animator;
    private bool hasShownUI;
    private Animator doorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        hasShownUI = false;
        UI = GameObject.Find("Canvas");
        animator = GetComponent<Animator>();
        animator.enabled = false;
        //doorAnimator = elevatorDoor.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (innerButton.GetComponent<ElevatorButtonBehavior>().GetButtonPress())
        {
            animator.enabled = true;
            
            
            
            if (!hasShownUI)
            {
                animator.SetBool("open", true);
                Invoke("ActivePlayerFinishUI", 1.5f);
                hasShownUI = true;
            }
            
        }
    }
    private void ActivePlayerFinishUI()
    {
        UI.GetComponent<MsgDisplayer>().ActivePlayerFinishUI();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Debug.Log("Lock state: " + Cursor.lockState);
    }
}

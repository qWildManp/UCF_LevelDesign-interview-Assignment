using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenColo : MonoBehaviour
{
    [SerializeField] public Transform door;
    [SerializeField] public Animator animator;
    public bool doorStat;
    // Start is called before the first frame update
    void Start()
    {
        animator = door.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("DoorOpen"))
        {
            GetComponent<Renderer>().material.color  = Color.green;
        }
        else
        {

            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}

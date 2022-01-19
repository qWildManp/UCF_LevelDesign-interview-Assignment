using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeDoubleDoorBehavior : InteractiveItem
{
    private Animator leftDooranimator;
    private Animator rightDooranimator;
    // Start is called before the first frame update
    void Awake()
    {
        SetHightLight(false);
        isChecked = false;
        leftDooranimator = transform.GetChild(0).GetComponent<Animator>();
        rightDooranimator = transform.GetChild(1).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChecked)
        {
            SetHightLight(true);
        }
        else
        {
            SetHightLight(false);
        }
        isChecked = false;
        player = GameObject.Find("Player");
            if (player)
            {
            Ray ray = player.GetComponent<PlayerRayCast>().GetPlayerRay();
            RaycastHit hit;
            LayerMask layerMask = ~(1 << 9);
            if (Physics.Raycast(ray, out hit, 120,layerMask))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                    isChecked = true;
                    if (Input.GetKeyDown(KeyCode.E))
                        {
                            ChangeDoorsStatus();
                        }
                    }
                }

            } 
    }
    void ChangeDoorsStatus()
    {
        leftDooranimator.SetBool("Open", !leftDooranimator.GetBool("Open"));
        rightDooranimator.SetBool("Open", !rightDooranimator.GetBool("Open"));
    }
}

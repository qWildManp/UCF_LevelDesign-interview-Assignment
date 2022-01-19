using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeSingleDoorBehavior : InteractiveItem
{
    private Animator dooranimator;
    // Start is called before the first frame update
    void Awake()
    {
        dooranimator = GetComponent<Animator>();
        isChecked = false;
        SetHightLight(false);
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
                            ChangeDoorStatus();
                        }
                    }

                }
            }        
    }
    void ChangeDoorStatus()
    {
        dooranimator.SetBool("Open", !dooranimator.GetBool("Open"));
    }
}

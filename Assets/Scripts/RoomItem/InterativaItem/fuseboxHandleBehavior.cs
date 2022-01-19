using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuseboxHandleBehavior : InteractiveItem
{
    // Start is called before the first frame update
    private bool isOff;
    void Start()
    {
        itemAnimator = GetComponent<Animator>();
        isChecked = false;
      
    }

    // Update is called once per frame
    void Update()
    {
        if (isChecked && !itemAnimator.GetBool("Close"))
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
            playerCapsule = player.transform.GetChild(2);
            playerCamera = player.transform.GetChild(0);
            Ray ray = player.GetComponent<PlayerRayCast>().GetPlayerRay();
            RaycastHit hit;
            LayerMask layerMask = ~(1 << 9);
            if (Physics.Raycast(ray, out hit, 120,layerMask))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isChecked = true;
                    if (Input.GetKeyDown(KeyCode.E)&&transform.parent)
                    {
                        Debug.Log("TurnOff");
                        TurnOff();
                    }
                }

            }
        }
    }
    private void TurnOff()
    {
        itemAnimator.SetBool("Close", true);
        isOff = true;
    }
    public bool GetIsOff()
    {
        return this.isOff;
    }
}

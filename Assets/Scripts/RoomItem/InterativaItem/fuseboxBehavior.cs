using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuseboxBehavior : InteractiveItem
{
    GameObject UI;
    [SerializeField] private bool open;
    [SerializeField] private bool hasHandle;
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("Canvas");
        open = true;
        hasHandle = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (hasHandle)
        {
            GameObject handle = transform.GetChild(0).gameObject;
            handle.SetActive(true);
            if (handle.GetComponent<fuseboxHandleBehavior>().GetIsOff())
                SetOpenStatus(false);
        }
        if (isChecked && !hasHandle)
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
            Ray ray = new Ray(playerCamera.position, playerCamera.forward * 120);
            RaycastHit hit;
            LayerMask layerMask = ~(1 << 9);
            if (Physics.Raycast(ray, out hit, 120,layerMask))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isChecked = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        RestoreFuseSwitch();
                        if (msg!="")
                        {
                            UI.GetComponent<MsgDisplayer>().SetMessage(msg);
                        }
                    }
                }

            }
        }
    }
    public void SetOpenStatus(bool result)
    {
        this.open = result;
    }
    public bool GetOpenStatus()
    {
        return this.open;
    }
    private void RestoreFuseSwitch()
    {
        GameObject handleInInventary = GameObject.Find("PlayerInventary").GetComponent<PlayerInventary>().CheckItem("FUSE HANDLE");
        if (handleInInventary != null)
        {
            msg = "";
            GameObject.Find("PlayerInventary").GetComponent<PlayerInventary>().UseItem(handleInInventary);
            this.hasHandle = true;
        }
        else
        {
            msg = "I need a FUSE HANDLE !";
        }
    }
}

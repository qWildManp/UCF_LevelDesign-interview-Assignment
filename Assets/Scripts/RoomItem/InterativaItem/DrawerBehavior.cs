using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawerBehavior : InteractiveItem
{
    // Start is called before the first frame update
    [SerializeField] GameObject openPanel;
    void Awake()
    {
        itemAnimator = GetComponent<Animator>();
        isChecked = false;
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
                            ChangeDrawerStatus();
                        }
                    }

                }
            }
        
    }
    public void UpdateOpenPanelText(string msg)
    {
        Text text = openPanel.GetComponent<Text>();
        text.text = msg;
    }
    public void ChangeDrawerStatus()
    {
        itemAnimator.SetBool("DrawerOpen", !itemAnimator.GetBool("DrawerOpen"));
    }
    // Update is called once per frame
}

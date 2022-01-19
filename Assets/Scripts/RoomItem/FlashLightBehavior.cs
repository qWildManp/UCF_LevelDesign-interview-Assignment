using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightBehavior : MonoBehaviour
{
    GameObject UI;
    [SerializeField]private bool open;
    [SerializeField]private int battery;
    [SerializeField] private int max_battery;
    [SerializeField]private float countDown;
    private float currentCountDown;
    [SerializeField]public bool inPlayerHand;
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("Canvas");
        currentCountDown = countDown;
        inPlayerHand = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(open && inPlayerHand)
            currentCountDown -= Time.deltaTime;
        if (currentCountDown <= 0)
        {
            Debug.Log("Battery loss");
            if (battery > 0)
            {
                battery -= 1;
            }

            currentCountDown = countDown;
        }
        if(battery <= 0)
        {
            open = false;
        }
        if (Input.GetKeyDown(KeyCode.L) && inPlayerHand)
        {
            if (battery > 0)
                open = !open;
            else
                UI.GetComponent<MsgDisplayer>().SetMessage("I need Battery !");
        }
        if (open)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public int GetCurrentBatteryInfo()
    {
        return this.battery;
    }
    public int GetMaxBatteryInfo()
    {
        return this.max_battery;
    }
    public bool AddBattery()
    {
        if (this.battery < max_battery)
        {
            this.battery += 1;
            return true;
        }
        UI.GetComponent<MsgDisplayer>().SetMessage("Battery is full ");
        return false;
    }
}

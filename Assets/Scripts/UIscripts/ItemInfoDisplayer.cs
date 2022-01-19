using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
public class ItemInfoDisplayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]public GameObject currentDisplayItem;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");
        GameObject player_flashlight = player.transform.Find("PlayerCapsule/PlayerCameraRoot/playerflashlight").gameObject;
        if (player_flashlight.activeInHierarchy)
        {
            Transform target_battery_remain = transform.Find("BatteryRemain_text/remain_num");
            int batteryInfo = player_flashlight.GetComponent<FlashLightBehavior>().GetCurrentBatteryInfo();
            target_battery_remain.GetComponent<Text>().text = batteryInfo.ToString();
        }
    }
    public void ShowInfo(GameObject itemDisplayer)
    {
        bool unknownItem = itemDisplayer.transform.Find("Unknown").gameObject.activeInHierarchy;
        if (unknownItem)
            return;
        GameObject item = itemDisplayer.GetComponent<ItemDisplayer>().GetItem();
        currentDisplayItem = item;
        PlayerInventary playerInventary = GameObject.Find("PlayerInventary").GetComponent<PlayerInventary>();
        if (item.GetComponent<RoomItem>().GetItemType() == RoomItemType.FLASHLIGHT)
        {
            transform.Find("BatteryRemain_text").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("BatteryRemain_text").gameObject.SetActive(false);
        }
        Sprite img =  itemDisplayer.GetComponent<Image>().sprite;
        Transform target_name = transform.Find("ItemName_text/ItemName");
        Transform target_img = transform.Find("ItemInfoImg");
        Transform target_describtion = transform.Find("Describtion");
        
        target_img.GetComponent<Image>().sprite = img;
        target_name.GetComponent<Text>().text = item.GetComponent<RoomItem>().GetItemName();
        target_describtion.GetComponent<Text>().text = item.GetComponent<RoomItem>().GetItemDescribtion();
    }
}

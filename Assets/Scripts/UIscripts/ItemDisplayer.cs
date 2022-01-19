using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayer : MonoBehaviour
{
    [SerializeField] GameObject itemPrefabe;
    [SerializeField] GameObject inventary;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (itemPrefabe)
        {
            string item_name = itemPrefabe.GetComponent<RoomItem>().GetItemName();
            bool playerHasItem = inventary.GetComponent<PlayerInventary>().FindItem(item_name);
            transform.Find("Unknown").gameObject.SetActive(!playerHasItem);
            transform.Find("ItemNum").gameObject.SetActive(playerHasItem);
            if (playerHasItem)
            {
                int itemNum = inventary.GetComponent<PlayerInventary>().GetItem(itemPrefabe).Value;
                transform.Find("ItemNum").GetComponent<Text>().text = itemNum.ToString();
            }
        }
       
    }
    public GameObject GetItem()
    {
        GameObject item = inventary.GetComponent<PlayerInventary>().GetItem(itemPrefabe).Key;
        return item;
    }
}

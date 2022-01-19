using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomItem : InteractiveItem
{
    [SerializeField] private RoomItemType itemType;
    [SerializeField] private string itemName;
    [SerializeField] string description;
    private Transform SpawnAt;
    // Start is called before the first frame update
    void Awake()
    {
        isChecked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnAt)
        {
            transform.position = SpawnAt.transform.position;
        }
        if (isChecked)
        {
            SetHightLight(true);
        }
        else
        {
            SetHightLight(false);
        }
        isChecked = false;
            
    }
    public RoomItemType GetItemType()
    {
        return this.itemType;
    }
    public string GetItemName()
    {
        return this.itemName;
    }
    public string GetItemDescribtion()
    {
        return this.description;
    }
    public void SetItemDescribtion(string msg)
    {
        this.description = msg;
    }
    public void SetSpawnAt(Transform spawnPoint)
    {
        this.SpawnAt = spawnPoint;
    }
}

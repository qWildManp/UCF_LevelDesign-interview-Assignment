using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemsSpawnRule", menuName = "ItemsSpawnRule")]

public class ItemsSpawnRuleSriptable : ScriptableObject
{
    public int totalItemsInSublevel;
    public int maxKeyCount;
    public int maxToolCount;
    public RoomItemType levelKeyItem;
    Dictionary<RoomItemType, int> avaliableItemNums;
    // Start is called before the first frame update
    void Start()
    {
        avaliableItemNums = new Dictionary<RoomItemType, int>();
        avaliableItemNums.Add(RoomItemType.KEY, this.maxKeyCount);
        avaliableItemNums.Add(RoomItemType.TOOLS, this.maxToolCount);
    }
    public void CloneAvailableItemDictionary(out Dictionary<RoomItemType, int> items)
    {
        items = new Dictionary<RoomItemType, int>();
        items.Add(RoomItemType.KEY, this.maxKeyCount);
        items.Add(RoomItemType.TOOLS, this.maxToolCount);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

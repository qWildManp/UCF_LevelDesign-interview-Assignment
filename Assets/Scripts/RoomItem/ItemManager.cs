using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RoomItemType
{
	KEY = 1,
	FLASHLIGHT = 2,
	TOOLS = 4,
	CLUE = 8,
	BATTERY = 16,
	HEALING = 32
}


public class ItemManager : MonoBehaviour
{

	private static ItemManager _instance;

	[SerializeField] private GameObject keyPrefab;
	[SerializeField] private List<GameObject> toolsPrefab;
	[SerializeField] private List<ItemsSpawnRuleSriptable> ruleList;
	[SerializeField] private List<RoomItemType> typeList;
	ItemsSpawnRuleSriptable activeItemSpawnRule;
	Dictionary<RoomItemType, int> avaliableItemList;
	public static ItemManager Instance { get { return _instance; } }

    private void Awake()
	{
		if (_instance !=null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
    }

	public void SpawnItemAt(RoomItemType type, Transform spawnPoint)
    {
		GameObject spawnedPrefab;
        switch (type)
        {
			case RoomItemType.KEY:
				spawnedPrefab = Instantiate(keyPrefab);
				break;
			case RoomItemType.TOOLS:
				int rnd_index = UnityEngine.Random.Range(0, toolsPrefab.Count);
				spawnedPrefab = Instantiate(toolsPrefab[rnd_index]);
				break;
			default:
				spawnedPrefab = Instantiate(keyPrefab);
				break;
        }
		//Instantiate
		//Debug.Log("spawn item name :" + spawnedPrefab.name);
		//Debug.Log("spawn at :" + spawnPoint.name);
		//Debug.Log("Spawn point position : " + spawnPoint.position.ToString());
		spawnedPrefab.transform.position = spawnPoint.position;
		spawnedPrefab.GetComponent<RoomItem>().SetSpawnAt(spawnPoint);
	}
	List<Transform> ShuffleList(List<Transform> list)
    {
		List<Transform> tmpList = list;
		for (int i = 0; i < tmpList.Count; i++)
        {
			int rnd_index = UnityEngine.Random.Range(0, tmpList.Count);
			Transform val = tmpList[i];
			tmpList[i] = tmpList[rnd_index];
			tmpList[rnd_index] = val;
		}
		return tmpList;
	}
	bool CheckValidation(RoomItemType itemType,Transform spawnPoint)
    {
		bool isTypeValidate;
		bool isSpawnPointValidate;
		int avaliableNum;
		avaliableItemList.TryGetValue(itemType, out avaliableNum);
		//check for avaliable num
		if (avaliableNum <= 0)
			isTypeValidate = false;
		else
			isTypeValidate = true;
		//check for spawn point is Okay,because some key item cannot spawn in checkpoint room
		if (spawnPoint.parent.name == "checkpointroom(Clone)" && itemType == activeItemSpawnRule.levelKeyItem)
			isSpawnPointValidate = false;
		else
			isSpawnPointValidate = true;
		// if both are validate return true
		if (isTypeValidate && isSpawnPointValidate)
			return true;
		else
			return false;

    }
	public void GenerateRoomItems(List<Transform> respawnPointsList, int level_no)
    {
		List<Transform> shuffledList = ShuffleList(respawnPointsList);
		if(level_no > ruleList.Count)
        {
			Debug.Log("Rule not set,skip this level");
			return;
        }
		activeItemSpawnRule = ruleList[level_no - 1];
		activeItemSpawnRule.CloneAvailableItemDictionary(out avaliableItemList);
		int maxCount = activeItemSpawnRule.totalItemsInSublevel;
		int generateNum = 0;

		//TODO : 理论上这个循环不会触发成死循环，但是这个循环体造成了卡死
        while (generateNum < maxCount)
        {
			if (shuffledList.Count == 0)
				break;
			int rnd_type_idx = UnityEngine.Random.Range(0, typeList.Count);
			int rnd_spawnpoint_idx = UnityEngine.Random.Range(0, shuffledList.Count);
			RoomItemType itemType = typeList[rnd_type_idx];
			Transform spawnPoint = shuffledList[rnd_spawnpoint_idx];
			if(CheckValidation(itemType, spawnPoint))
            {
				generateNum += 1;
				SpawnItemAt(itemType, spawnPoint);
				avaliableItemList[itemType] -= 1;
				shuffledList.Remove(spawnPoint);
            }
		}

    }
}


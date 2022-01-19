using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject DoorObject;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDoorObject(GameObject obj)
    {
        this.DoorObject = obj;
    }
    public GameObject GetDoorObject()
    {
        return this.DoorObject;
    }
    public GameObject GenerateRoom(GameObject roomPrefab)
    {
        var roomInstance = Instantiate(roomPrefab);
        return roomInstance;
    }

}

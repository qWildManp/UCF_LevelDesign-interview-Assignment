using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum RoomType
{
    END = 1,
    START = 2,
    EXPLORE = 4,
    DANGER = 8,
    PUZZLE = 16,
    FINAL_PUZZLE = 32,
    CHECKPOINT = 64
};
public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform[] roomExits;
    [SerializeField] private Transform[] itemSpawnPoints;
    [SerializeField] private string roomName;
    private List<Transform> exitsList;
    [SerializeField] private RoomType roomType;
    private Vector2 roomID;
    private Dictionary<Transform,GameObject> exitConnectingTo;
    private Transform parentExit;
    void Awake()
    {
        exitsList = new List<Transform>();
        exitConnectingTo = new Dictionary<Transform, GameObject>();
        foreach(var exit in roomExits)
        {
            exitsList.Add(exit);
            exitConnectingTo.Add(exit, null);
        }

    }
    public void ConstructExitList()
    {
        exitsList = new List<Transform>();
        foreach (var exit in roomExits)
        {
            exitsList.Add(exit);
        }
    }
    public Dictionary<Transform, GameObject> getExitConnectingTo()
    {
        return exitConnectingTo;
    }
    public void setConnection(Transform currentExitTransform, Transform connectingExitTransform)
    {
        this.exitConnectingTo[currentExitTransform] = connectingExitTransform.parent.gameObject;
    }
    public void SetGeneratedFrom(Transform parentExit)
    {
        this.parentExit = parentExit;
    }
    public Transform GetParentExit()
    {
        return parentExit;
    }
    public GameObject GetConnectionOfSingleExit(Transform exit)
    {
        return exitConnectingTo[exit];
    }
    /*
     * 
     *   +------+
     *   |      |
     *   |      T  -- currentExitTransform (现有的房间的出口)
     *   |      |
     *   +------+
     *              -- connectingExitTransform (新生成的房间的出口)
     * 
     * 
     */

    public void PositionToExit(Transform currentExitTransform, Transform connectingExitTransform)
    {
        //Debug.Log(exitConnectingTo[currentExitTransform]);
        connectingExitTransform.SetParent(null, true);
        transform.SetParent(connectingExitTransform, true);


        connectingExitTransform.position = currentExitTransform.position;
        connectingExitTransform.forward = -currentExitTransform.forward;




        transform.SetParent(null, true);
        transform.position = transform.position;
        exitsList.Remove(connectingExitTransform);
        exitConnectingTo.Remove(connectingExitTransform);
        Destroy(connectingExitTransform.gameObject);
        //currentExitTransform.GetComponent<Renderer>().enabled = false;

    }
    public void setRoomID(Vector2 ID)
    {
        roomID = ID;
    }
    public Vector2 getRoomID()
    {
        return roomID;
    }
    public List<Transform> GetAllAvaliableExit()
    {
        return exitsList;
    }
    public Transform GetRandomExit()
    {
        int exitCount = exitsList.Count;
        int exitId = UnityEngine.Random.Range(0, exitCount);
        return exitsList[exitId];
    }

    public Transform GetFirstExit()
    {
        return exitsList[0];
    }

    public RoomType GetRoomType()
    {
        return this.roomType;
    }
    
    public Transform[] GetItemSpawnPoints()
    {
        return itemSpawnPoints;
    }

    public string GetRoomName()
    {
        return this.roomName;
    }
}

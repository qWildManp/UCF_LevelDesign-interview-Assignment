using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [System.Serializable]
    public struct RoomDataBundle
    {
        public List<GameObject> roomPrefabs;
        public bool allowReuse;
    }



    private static RoomManager _instance;

    [SerializeField] private List<GameObject> explorRoomList;
    [SerializeField] private List<GameObject> dangerRoomList;
    [SerializeField] private List<GameObject> puzzleRoomList;
    [SerializeField] private List<GameObject> finalPuzzleRoomList;
    [SerializeField] private List<RoomRulesScriptable> ruleList;
    [SerializeField] GameObject startRoomPrefab;
    [SerializeField] GameObject endRoomPrefabe;
    [SerializeField] GameObject checkpointRoomPrefabe;
    [SerializeField] GameObject checkpointRoomStairPrefabe;
    [SerializeField] GameObject player;
    [SerializeField] GameObject doorPrefabe;
    [SerializeField] GameObject lunaDoorPrefabe;
    [SerializeField] GameObject checkpointDoorPrefabe;
    [SerializeField] GameObject brokenDoorPrefabe;

    [SerializeField] int seed = 0;
    [SerializeField] bool generateRandomSeed;

    public static RoomManager Instance { get { return _instance; } }
    private List<RoomGenerator> exitsToGenerate;
    private List<Transform> avaliableExits;
    private List<Transform> previousAvaliableExits;
    private Dictionary<int, RoomGenerateTask> roomTasks;
    private RoomDataBundle exploreBundle;
    private RoomDataBundle dangerBundle;
    private RoomDataBundle puzzleBundle;
    private RoomDataBundle finalPuzzleBundle;
    private Dictionary<RoomType, int> availableRoomNums;
    private Dictionary<RoomType, RoomDataBundle> roomsOfSubtype;
    private Dictionary<int, List<GameObject>> roomsOfSublevel;

    private Transform workingExit;
    private Room previousRoom;
    private RoomRulesScriptable activeRule;
    private RoomRulesScriptable previousRule;

    private int generatedRoomCount = 0;
    private int previousRoomCount = 0;
    private int task_id = 0;
    private int ruleIndex = 0;
    private bool hasOverlap = false;
    private int badGeneration = 0;
    public int currentTotalRoomNum = 0;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        avaliableExits = new List<Transform>();
        previousAvaliableExits = new List<Transform>();
        exitsToGenerate = new List<RoomGenerator>();
        roomTasks = new Dictionary<int, RoomGenerateTask>();

    }

    private void Start()
    {
        roomsOfSubtype = new Dictionary<RoomType, RoomDataBundle>();
        roomsOfSublevel = new Dictionary<int, List<GameObject>>();
        exploreBundle.roomPrefabs = explorRoomList;
        exploreBundle.allowReuse = false;
        dangerBundle.roomPrefabs = dangerRoomList;
        dangerBundle.allowReuse = true;
        puzzleBundle.roomPrefabs = puzzleRoomList;
        puzzleBundle.allowReuse = true;
        finalPuzzleBundle.roomPrefabs = finalPuzzleRoomList;
        finalPuzzleBundle.allowReuse = true;

        roomsOfSubtype.Add(RoomType.EXPLORE, exploreBundle);
        roomsOfSubtype.Add(RoomType.DANGER, dangerBundle);
        roomsOfSubtype.Add(RoomType.PUZZLE, puzzleBundle);
        roomsOfSubtype.Add(RoomType.FINAL_PUZZLE, finalPuzzleBundle);

        activeRule = ruleList[ruleIndex];
        activeRule.CloneAvailableRoomDictionary(out availableRoomNums);
        roomsOfSublevel.Add(activeRule.sublevel_no, new List<GameObject>());
        workingExit = GenerateStartRoom(activeRule);

        AddTask(new RoomGenerateTask(RoomTask.TaskType.GENERATE_NEW_ROOM));
        if (generateRandomSeed)
        {
            seed = System.DateTime.UtcNow.Millisecond;
            Debug.Log("new seed: " + seed);
        }
        UnityEngine.Random.InitState(seed);
    }
    //Generate start room
    private Transform GenerateStartRoom(RoomRulesScriptable rule)
    {

        //generate start room
        GameObject startRoom = Instantiate(startRoomPrefab);
        addAvaliableExits(startRoom.GetComponent<Room>().GetAllAvaliableExit());
        var currentExit = getRandomexitfromExitPool();
        startRoom.GetComponent<Room>().setRoomID(new Vector2(rule.sublevel_no, 0));
        roomsOfSublevel[rule.sublevel_no].Add(startRoom);
        return currentExit;
    }

    //Generate checkpoint room
    public void GenerateCheckpointRoom()
    {
        // generate stairs
        Transform currentExit = null;
        if (previousRoom)
        {
            currentExit = previousRoom.GetComponent<Room>().GetRandomExit();
        }
        if (workingExit)
        {
            currentExit = workingExit;
        }
        GameObject stair = currentExit.GetComponent<RoomGenerator>().GenerateRoom(checkpointRoomStairPrefabe);
        Transform stairExit = stair.GetComponent<Room>().GetRandomExit();
        currentExit.parent.GetComponent<Room>().setConnection(currentExit, stairExit);// record connection between two rooms
        stair.GetComponent<Room>().PositionToExit(currentExit, stairExit);
        stair.GetComponent<Room>().setRoomID(new Vector2(previousRule.sublevel_no, generatedRoomCount + 1));
        roomsOfSublevel[previousRule.sublevel_no].Add(stair);
        removeUsedExit(currentExit);
        currentExit = stair.GetComponent<Room>().GetRandomExit();
        //generate checkpoint room
        GameObject checkpointRoom = currentExit.GetComponent<RoomGenerator>().GenerateRoom(checkpointRoomPrefabe);
        Transform checkpointRoomExit = checkpointRoom.GetComponent<Room>().GetRandomExit();
        currentExit.parent.GetComponent<Room>().setConnection(currentExit, checkpointRoomExit);// record connection between two rooms
        checkpointRoom.GetComponent<Room>().PositionToExit(currentExit, checkpointRoomExit);
        checkpointRoom.GetComponent<Room>().setRoomID(new Vector2(previousRule.sublevel_no, generatedRoomCount + 1));
        roomsOfSublevel[previousRule.sublevel_no].Add(checkpointRoom);
        addAvaliableExits(checkpointRoom.GetComponent<Room>().GetAllAvaliableExit());
        removeUsedExit(currentExit);
        removeUsedExit(checkpointRoomExit);
        //workingExit = checkpointRoom.GetComponent<Room>().GetRandomExit();
        AddTask(new RoomGenerateTask(RoomTask.TaskType.GENERATE_NEW_ROOM));
        SetLastRoom(checkpointRoom.GetComponent<Room>());
    }

    [SerializeField] private float countDown = 1f;
    private float currentCountDown = 1f;
    public bool hasGenerateRooms = false;
    public bool hasGeneratedItems = false;
    public bool hasGeneratedDoors = false;
    public bool hasGeneratedCharacter = false;
    public bool hasInitializeEndRoom = false;
    public bool hasFinishInitialize = false;
    private void Update()
    {
        currentCountDown -= Time.deltaTime;
        RoomGenerateTask activeTask;
        if (currentCountDown <= 0)
        {
            if (roomTasks.Count > 0)
            {
                //Debug.Log(previousRoom);
                if (hasOverlap)
                {
                    badGeneration += 1;
                    if (badGeneration > 10)
                    {
                        Debug.Log("Bad checkpoint room generate, first delete last room");
                        DestroyLastRoom(false);
                        badGeneration = 0;
                    }
                    Debug.Log("Overlap occur!!!");
                    roomTasks.Clear();
                    AddTask(new RoomGenerateTask(RoomTask.TaskType.DELETE_LAST_ROOM));
                    activeTask = roomTasks[task_id];
                    roomTasks.Remove(task_id);
                    activeTask.ExecuteTask();
                }
                else
                {
                    refreashAvaliableExits(activeRule);
                    workingExit = getRandomexitfromExitPool();
                    if (workingExit == null)
                    /*For sublevel >= 2
                     *after delete room it is the first room in this sublevel and working exit is lost due to the deletion
                     *find the previous checkpoint room exit as woriking exit
                     */
                    {
                        workingExit = roomsOfSublevel[previousRule.sublevel_no].Last().GetComponent<Room>().GetRandomExit();
                    }
                    if (generatedRoomCount >= activeRule.totalRoomsInSublevel)
                    {
                        if (this.ruleIndex + 1 < ruleList.Count)
                        {
                            // In case that checkpoint room have overlap with other room and need it to reset to previous state
                            previousRule = activeRule;
                            previousRoomCount = generatedRoomCount;
                            previousAvaliableExits.Clear();
                            avaliableExits.ForEach(item => previousAvaliableExits.Add(item));
                            avaliableExits.Clear();
                            roomTasks.Clear();
                            AddTask(new RoomGenerateTask(RoomTask.TaskType.GENERATE_CHECKPOINT_ROOM));
                            this.ruleIndex += 1;
                            activeRule = ruleList[this.ruleIndex];
                            activeRule.CloneAvailableRoomDictionary(out availableRoomNums);
                            generatedRoomCount = 0;
                            if (!roomsOfSublevel.ContainsKey(activeRule.sublevel_no))
                            {
                                roomsOfSublevel.Add(activeRule.sublevel_no, new List<GameObject>());
                            }

                            Debug.Log("set rulset to " + activeRule.sublevel_no.ToString());
                            return;
                        }
                        else
                        {
                            hasGenerateRooms = true;
                            //Generate Item
                            if (!hasGeneratedItems)
                            {
                                Debug.Log("Generate Items");
                                foreach (KeyValuePair<int, List<GameObject>> levelRoomList in roomsOfSublevel)
                                {
                                    List<Transform> respawnPointsList = new List<Transform>();
                                    foreach (GameObject room in levelRoomList.Value)
                                    {
                                        Room roomMonoBehavior = room.GetComponent<Room>();
                                        if (roomMonoBehavior)
                                        {
                                            var roomSpawnPoints = roomMonoBehavior.GetItemSpawnPoints();
                                            foreach(var point in roomSpawnPoints)
                                            {
                                                respawnPointsList.Add(point);
                                            }
                                        }
                                    }
                                    GameObject.Find("ItemManager").GetComponent<ItemManager>().GenerateRoomItems(respawnPointsList, levelRoomList.Key);
                                    Debug.Log("level " + levelRoomList.Key + "over");
                                }
                                hasGeneratedItems = true;
                            }
                            //Generate Door
                            if (!hasGeneratedDoors)// Generate Doors
                             {
                                    List<Transform> entrances;
                                    foreach (var sublevel in roomsOfSublevel)
                                    {
                                        foreach (var room in sublevel.Value)
                                        {
                                            entrances = room.GetComponent<Room>().GetAllAvaliableExit();
                                            foreach (Transform exit in entrances)
                                            {
                                                exit.GetComponent<Renderer>().enabled = false;
                                                Vector3 exit_pos = exit.transform.position;
                                                Vector3 exit_dir = exit.transform.right;
                                                GameObject connection = exit.parent.GetComponent<Room>().GetConnectionOfSingleExit(exit);
                                                GameObject door;
                                                if (connection == null)
                                                {
                                                    door = Instantiate(brokenDoorPrefabe);
                                                    door.GetComponent<DoorBehavior>().SetDoorOpenable(false);
                                                }
                                                else
                                                {
                                                    if (connection.name == "checkpointroom_stair(Clone)")
                                                    {
                                                        if (sublevel.Key == 2)
                                                            door = Instantiate(lunaDoorPrefabe);
                                                        else
                                                            door = Instantiate(checkpointDoorPrefabe);
                                                    }
                                                    else
                                                    {
                                                        door = Instantiate(doorPrefabe);
                                                    }
                                                }
                                                exit.GetComponent<RoomGenerator>().SetDoorObject(door);
                                                door.transform.position = new Vector3(exit_pos.x, exit_pos.y + 70f, exit_pos.z);
                                                door.transform.forward = exit_dir;
                                            }
                                        }
                                    }
                                    hasGeneratedDoors = true;
                                }
                            //Generate character
                            if (!hasGeneratedCharacter)
                                {
                                    Destroy(GameObject.Find("Camera"));
                                    player.SetActive(true);
                                    //player.transform.position = new Vector3(24f, -142f, -6f);
                                    hasGeneratedCharacter = true;
                                }
                            if (!hasInitializeEndRoom)
                            {
                                GameObject.Find("endroom(Clone)").GetComponent<EndRoomBehavior>().GetTwoPuzzleRooms();
                            }
                            hasFinishInitialize = true;
                            return;
                        }
                    }
                    activeTask = roomTasks[task_id];
                    roomTasks.Remove(task_id);
                    activeTask.ExecuteTask();
                }
                this.hasOverlap = false;
            }
            currentCountDown = countDown;
        }
    }

        public void AddToGenerationList(RoomGenerator exit)
        {
            exitsToGenerate.Add(exit);
        }

        public void AddTask(RoomGenerateTask task)
        {
            this.task_id += 1;
            roomTasks.Add(task_id, task);
        }
        public void overlapDetect()
        {
            this.hasOverlap = true;
        }


        public void GenerateSingleRoomWithRuleset()// Generate a new room
        {
            var rule = activeRule;
            var currentExit = workingExit;
            if (generatedRoomCount >= rule.totalRoomsInSublevel)
            {
                return;
            }

            GameObject pickedRoomPrefab;
            GameObject connectingRoom;
            int randomIndex = 0;
            RoomType pickedType;
            List<RoomType> availableRoomtype = new List<RoomType>();
            availableRoomtype.AddRange(rule.avaliableRoomType);

            if (generatedRoomCount == 0 && ruleIndex == 3) { // if it is the last level,first generate end room
                pickedType = RoomType.END;
                pickedRoomPrefab = endRoomPrefabe;
            }
            else
            {
                randomIndex = randomNumber(availableRoomtype.Count);
                pickedType = availableRoomtype[randomIndex];



                // find a suitable room type
                while (!RoomFitsRuleset(pickedType, availableRoomNums) && availableRoomtype.Count > 0)
                {
                    availableRoomtype.Remove(pickedType);
                    if (availableRoomtype.Count <= 0)
                    {

                        return;
                    }
                    randomIndex = randomNumber(availableRoomtype.Count);
                    pickedType = availableRoomtype[randomIndex];
                }


                //generate new room
                pickedRoomPrefab = PickRandomRoomfromType(pickedType);
            }
            connectingRoom = currentExit.GetComponent<RoomGenerator>().GenerateRoom(pickedRoomPrefab);
            Transform connectingExit = connectingRoom.GetComponent<Room>().GetRandomExit(); //connectingExit - 新生成房间的出口，用来连接到currentExit
            workingExit.parent.GetComponent<Room>().setConnection(workingExit, connectingExit);//parent room record connection between new rooms
            connectingRoom.GetComponent<Room>().PositionToExit(currentExit, connectingExit);//connect two rooms
            connectingRoom.GetComponent<Room>().SetGeneratedFrom(workingExit);//new generated room record parent exit                                                                                //record new generated room is from which room
            connectingRoom.GetComponent<Room>().setRoomID(new Vector2(rule.sublevel_no, generatedRoomCount + 1));//set new room ID

        //add new room to the roomlist,add new avaliable exits and remove used exit
        roomsOfSublevel[rule.sublevel_no].Add(connectingRoom);
            addAvaliableExits(connectingRoom.GetComponent<Room>().GetAllAvaliableExit());
            removeUsedExit(currentExit);
            removeUsedExit(connectingExit);

            // Assign new task

            if (pickedType != RoomType.END)
            {
                availableRoomNums[pickedType] -= 1;
            }
            generatedRoomCount += 1;
            currentTotalRoomNum += 1;
            AddTask(new RoomGenerateTask(RoomTask.TaskType.GENERATE_NEW_ROOM));
            SetLastRoom(connectingRoom.GetComponent<Room>());
           
        }

        void PrintAllExitNames()
        {
            var outstring = "";
            for (var i = 0; i < avaliableExits.Count; i++)
            {
                outstring += avaliableExits[i].parent.name + " - " + avaliableExits[i] + "//";
            }
            Debug.Log(outstring);
        }

        public void DestroyLastRoom(bool regenerate = false)
        {
            if (previousRoom != null)
            {
                if (previousRoom.GetRoomType() == RoomType.CHECKPOINT && regenerate == false)
                {   // if checkpoint room overlap occur
                    //reset to last rule
                    Debug.Log("Checkpoint Room overlap occur,reset to last rule...");
                    ruleIndex -= 1;
                    activeRule = previousRule;
                    previousAvaliableExits.ForEach(i => avaliableExits.Add(i));
                    generatedRoomCount = previousRoomCount;
                }
                var previousRoomObject = roomsOfSublevel[activeRule.sublevel_no].Last();
                var previousRoomBehavior = previousRoomObject.GetComponent<Room>();
                if (previousRoomBehavior.GetRoomType() != RoomType.CHECKPOINT)// if the delete room is not checkpointroom recover the avaliable number of this type
                    availableRoomNums[previousRoomBehavior.GetRoomType()] += 1;
                Debug.Log("Destroyed Room" + previousRoomObject.name);
                //remove the record of the deleted room
                roomsOfSublevel[activeRule.sublevel_no].Remove(previousRoomObject);
                for (int i = Math.Max(avaliableExits.Count - 1, 0); i >= 0 && avaliableExits.Count > 0; i--)
                {
                    if (avaliableExits[i] && avaliableExits[i].parent)
                    {
                        if (avaliableExits[i].parent.GetComponent<Room>())
                        {
                            if (avaliableExits[i].parent.GetComponent<Room>().getRoomID() == previousRoomBehavior.getRoomID())
                            {

                                avaliableExits.Remove(avaliableExits[i]);
                            }

                        }

                    }
                    else
                    {
                        avaliableExits.Remove(avaliableExits[i]);
                    }
                }
                Destroy(previousRoomObject);
                if (previousRoom.GetRoomType() != RoomType.CHECKPOINT)// if destory room is checkpoint room do not changde the generated num
                {
                    generatedRoomCount -= 1;
                    currentTotalRoomNum -= 1;
                }
                //Add new task
                if (regenerate)
                {
                    RoomGenerateTask nextTask = new RoomGenerateTask(RoomTask.TaskType.GENERATE_NEW_ROOM);
                    AddTask(nextTask);
                    return;
                }


                if (roomsOfSublevel[activeRule.sublevel_no].Count > 0)
                {
                    DestroyLastRoom(true);
                    /* if after the second delete the current level room list is empty, 
                     * find the checkpoint room from the previous level as last room 
                     */
                    if (roomsOfSublevel[activeRule.sublevel_no].Count == 0)
                    {

                        SetLastRoom(roomsOfSublevel[previousRule.sublevel_no].Last().GetComponent<Room>());
                        return;
                    }
                    SetLastRoom(roomsOfSublevel[activeRule.sublevel_no].Last().GetComponent<Room>());
                }
                /* if after the first delete the current level room list is empty, 
                 * find the checkpoint room exit from the previous level
                 */
                else
                {

                    RoomGenerateTask nextTask = new RoomGenerateTask(RoomTask.TaskType.GENERATE_NEW_ROOM);
                    AddTask(nextTask);
                    SetLastRoom(roomsOfSublevel[previousRule.sublevel_no].Last().GetComponent<Room>());
                    return;

                }
            }
        }

        public void SetLastRoom(Room room)
        {
            previousRoom = room;
        }

        // Exit pool operations
        public Transform getRandomexitfromExitPool()
        {

            int randomExitId = randomNumber(avaliableExits.Count);
            if (avaliableExits.Count <= 0)
            {
                return null;
            }
            return avaliableExits[randomExitId];
        }
        public void addAvaliableExits(List<Transform> exitsOfOneRoom)
        {
            foreach (var exit in exitsOfOneRoom)
            {
                if (!avaliableExits.Contains(exit))
                {
                    avaliableExits.Add(exit);
                }
            }
        }
        public void refreashAvaliableExits(RoomRulesScriptable rule)
        {
            List<GameObject> roomlistOfSublevel = roomsOfSublevel[rule.sublevel_no];
            foreach (var room in roomlistOfSublevel)
            {
                Dictionary<Transform, GameObject> roomConnections = room.GetComponent<Room>().getExitConnectingTo();
                foreach (var roomConnection in roomConnections)
                {
                    if (roomConnection.Value == null)
                    {
                        var exit = roomConnection.Key;

                        if (!avaliableExits.Contains(exit))
                        {
                            avaliableExits.Add(exit);
                        }
                    }
                }
            }
        }
        public void removeUsedExit(Transform exit)
        {
            avaliableExits.Remove(exit);
        }


        //check picked room is valid
        public bool RoomFitsRuleset(RoomType roomtype, Dictionary<RoomType, int> avaliableRoomNums)
        {
            int avaliableNum;
            avaliableRoomNums.TryGetValue(roomtype, out avaliableNum);

            if (avaliableNum <= 0)
            {
                return false;

            }
            else if (roomtype == activeRule.specialRequirementType)// if rest avaliable room of picked type is one
            {

                foreach (var type in availableRoomNums)// if there is other roomtype have avaliable room return false
                {
                    if (type.Key != roomtype && type.Value > 0)
                    {
                        if (roomtype == RoomType.FINAL_PUZZLE)
                        {
                            return false;
                        }
                        else if (avaliableNum == 1)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return true;
            }


        }

        public GameObject PickRandomRoomfromType(RoomType type)
        {
            List<GameObject> validRooms = roomsOfSubtype[type].roomPrefabs;
            int listLen = validRooms.Count;
            GameObject pickedRoom = validRooms[randomNumber(listLen)];
            if (type == RoomType.PUZZLE | type == RoomType.FINAL_PUZZLE)
            {
                while (GameObject.Find(pickedRoom.name + "(Clone)"))
                {
                    pickedRoom = validRooms[randomNumber(listLen)];
                }
            }
            return pickedRoom;
        }
        public int randomNumber(int listLen)
        {
            return UnityEngine.Random.Range(0, listLen);
        }

    } 


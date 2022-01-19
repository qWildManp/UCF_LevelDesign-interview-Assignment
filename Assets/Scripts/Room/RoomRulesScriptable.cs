using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "RoomRules", menuName = "RoomRules")]
public class RoomRulesScriptable : ScriptableObject
{
    public int totalRoomsInSublevel = -1;
	public int maxDangerCount = -1;
	public int maxExplorCount = -1;
    public int maxPuzzleCount = -1;
	public int maxFinalPuzzleCount = -1;
    public int sublevel_no = -1;
    public RoomType[] avaliableRoomType = null;
	public RoomType specialRequirementType;
	Dictionary<RoomType, int> avaliableRoomNums;

	public onRoomsGenerated onRoomsGeneratedDelegate;
	public delegate void onRoomsGenerated();


	// Start is called before the first frame update
	void Start()
	{
		avaliableRoomNums = new Dictionary<RoomType, int>();
		avaliableRoomNums.Add(RoomType.EXPLORE, this.maxExplorCount);
		avaliableRoomNums.Add(RoomType.DANGER, this.maxDangerCount);
		avaliableRoomNums.Add(RoomType.PUZZLE, this.maxPuzzleCount);
		avaliableRoomNums.Add(RoomType.FINAL_PUZZLE, this.maxFinalPuzzleCount);
	}

	public void CloneAvailableRoomDictionary(out Dictionary<RoomType, int> rooms)
    {
		rooms = new Dictionary<RoomType, int>();
		rooms.Add(RoomType.EXPLORE, this.maxExplorCount);
		rooms.Add(RoomType.DANGER, this.maxDangerCount);
		rooms.Add(RoomType.PUZZLE, this.maxPuzzleCount);
		rooms.Add(RoomType.FINAL_PUZZLE, this.maxFinalPuzzleCount);
	}
}

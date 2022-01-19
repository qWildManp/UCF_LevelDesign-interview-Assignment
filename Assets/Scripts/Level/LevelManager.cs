using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject UI;
    private int playerInLevel;
    [SerializeField] private List<string> GameObjectiveInSubLevels;
    void Start()
    {
        UI = GameObject.Find("Canvas");
        playerInLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPlayerInLevel(int level)
    {
        //means player Enter the next sublevel
        if (level > playerInLevel) {
            int newLevel = level;
            UpdateMainGameObjective(newLevel);
            this.playerInLevel = level;
        }  
    }
    public void UpdateMainGameObjective(int newLevel)
    {
        string newLevelObjective = GameObjectiveInSubLevels[newLevel - 1];
        UI.GetComponent<MsgDisplayer>().SetMainObjective(newLevelObjective);
    }
    public int GetPlayerInLevel()
    {
        return this.playerInLevel;
    }
}

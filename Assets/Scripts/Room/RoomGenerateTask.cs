using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomTask
{
    public enum TaskType
    {
        GENERATE_NEW_ROOM,
        DELETE_LAST_ROOM,
        GENERATE_CHECKPOINT_ROOM,
        OUTPUT_TRASH,
        STOP
    }
}


public class RoomGenerateTask
{


    private RoomTask.TaskType currentType;


    public RoomGenerateTask(RoomTask.TaskType type)
    {
        this.currentType = type;
    }

    public RoomTask.TaskType GetTaskType()
    {
        return currentType;
    }

    public void ExecuteTask()
    {
        switch (currentType)
        {
            case RoomTask.TaskType.GENERATE_NEW_ROOM:
                RoomManager.Instance.GenerateSingleRoomWithRuleset();
                break;
            case RoomTask.TaskType.OUTPUT_TRASH:
                Debug.Log("TRASH");
                break;
            case RoomTask.TaskType.DELETE_LAST_ROOM:
                RoomManager.Instance.DestroyLastRoom();
                break;
            case RoomTask.TaskType.GENERATE_CHECKPOINT_ROOM:
                RoomManager.Instance.GenerateCheckpointRoom();
                break;
        }
        return;
    }
}

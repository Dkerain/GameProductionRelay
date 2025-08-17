using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TaskObjectiveType
{
    CollectItem,    //收集物品
    ReachLocation,  //到达指定地点
    InteractWith   //完成互动
}
[System.Serializable] // 使其在Unity编辑器中可显示
public struct TaskReward
{
    public int currency; // 奖励金币数量
    public string itemId; // 奖励物品ID (例如 "key_2floor")
    // 可以继续添加其他奖励类型如解锁能力等
}
[System.Serializable]
public class Task
{
    public string id;
    public string title;
    [TextArea(1, 3)]
    public string description;
    public TaskObjectiveType objectiveType;
    public string targetId;
    public int requiredCount;
    [HideInInspector]
    public int currentCount;
    [HideInInspector]
    public bool isLocationReached;
    [HideInInspector]
    public bool isInteracted;
    [HideInInspector]
    public bool isActive;
    [HideInInspector]
    public bool isCompleted;
    [HideInInspector]
    public bool isClaimed;
    public TaskReward reward;
    public void CheckCompletion()
    {
        if (isCompleted) return; // 如果已完成不再检查

        switch (objectiveType)
        {
            case TaskObjectiveType.CollectItem:
                isCompleted = (currentCount >= requiredCount);
                break;

            case TaskObjectiveType.ReachLocation:
                isCompleted = isLocationReached;
                break;

            case TaskObjectiveType.InteractWith:
                isCompleted = isInteracted;
                break;
        }
    }
}

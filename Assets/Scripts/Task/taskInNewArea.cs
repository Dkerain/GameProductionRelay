using UnityEngine;

public enum TaskObjectiveType
{
    CollectItem,
    ReachLocation,
    InteractWith
}

[System.Serializable]
public struct TaskReward
{
    public int currencyAmount;
    public string rewardItemId;
}

[System.Serializable]
public class Task
{
    // 任务标识符
    [Header("任务基本信息")]
    public string taskId;
    public string taskTitle;

    [TextArea(1, 3)]
    public string taskDescription;
    public override string ToString()
    {
        return $"Task '{taskTitle}' (ID: {taskId}) - Type: {taskObjectiveType}";
    }

    // 任务目标配置
    [Header("Task Objective")]
    public TaskObjectiveType taskObjectiveType;
    public string objectiveTargetId;
    public int requiredItemCount;

    // 任务状态 - 使用下划线前缀
    [Header("Task State (Runtime)")]
    [SerializeField, HideInInspector]
    private int _currentItemCount;

    [SerializeField, HideInInspector]
    private bool _isLocationReached;

    [SerializeField, HideInInspector]
    private bool _isObjectInteracted;

    [SerializeField, HideInInspector]
    private bool _isTaskActive;

    [SerializeField, HideInInspector]
    private bool _isTaskCompleted;

    [SerializeField, HideInInspector]
    private bool _isRewardClaimed;

    [Header("Task Reward")]
    public TaskReward taskReward;

    // 公共属性封装私有字段
    public int CurrentItemCount
    {
        get => _currentItemCount;
        set => _currentItemCount = value;
    }

    public bool IsLocationReached
    {
        get => _isLocationReached;
        set => _isLocationReached = value;
    }

    public bool IsObjectInteracted
    {
        get => _isObjectInteracted;
        set => _isObjectInteracted = value;
    }

    public bool IsTaskActive
    {
        get => _isTaskActive;
        set => _isTaskActive = value;
    }

    public bool IsTaskCompleted
    {
        get => _isTaskCompleted;
        set => _isTaskCompleted = value;
    }

    public bool IsRewardClaimed
    {
        get => _isRewardClaimed;
        set => _isRewardClaimed = value;
    }

    // 检查任务完成状态 - 方法名改为更具体的 EvaluateCompletion
    public void EvaluateCompletion()
    {
        if (this.IsTaskCompleted) return;

        switch (this.taskObjectiveType)
        {
            case TaskObjectiveType.CollectItem:
                this.IsTaskCompleted = (this.CurrentItemCount >= this.requiredItemCount);
                break;

            case TaskObjectiveType.ReachLocation:
                this.IsTaskCompleted = this.IsLocationReached;
                break;

            case TaskObjectiveType.InteractWith:
                this.IsTaskCompleted = this.IsObjectInteracted;
                break;
        }
    }

    // 获取任务进度文本
    public string GetProgressText()
    {
        switch (this.taskObjectiveType)
        {
            case TaskObjectiveType.CollectItem:
                return $"{this.CurrentItemCount}/{this.requiredItemCount}";

            case TaskObjectiveType.ReachLocation:
                return this.IsLocationReached ? "完成" : "未到达";

            case TaskObjectiveType.InteractWith:
                return this.IsObjectInteracted ? "完成" : "未互动";

            default:
                return "N/A";
        }
    }
}
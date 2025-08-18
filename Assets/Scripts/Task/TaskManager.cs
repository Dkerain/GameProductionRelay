using System.Collections.Generic;
using UnityEngine;
using Game.Tasks;
public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    [Header("TASK SETTINGS")]
    [SerializeField] private List<Task> _allTasks = new List<Task>();

    public List<Task> ActiveTasks { get; private set; } = new List<Task>();
    public List<Task> CompletedTasks { get; private set; } = new List<Task>();
    public List<Task> AllTasks => _allTasks;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeTasks();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeTasks()
    {
        foreach (var task in _allTasks)
        {
            task.IsTaskActive = false;
            task.IsTaskCompleted = false;
            task.IsRewardClaimed = false;
            task.CurrentItemCount = 0;
            task.IsLocationReached = false;
            task.IsObjectInteracted = false;
        }
    }

    public List<Task> GetAvailableTasks()
    {
        return _allTasks.FindAll(t =>
            !t.IsTaskActive &&
            !t.IsTaskCompleted &&
            !t.IsRewardClaimed);
    }

    public bool HasNewTasks() => GetAvailableTasks().Count > 0;

    public void AcceptTask(Task task)
    {
        if (task == null || task.IsTaskActive) return;

        task.IsTaskActive = true;
        ActiveTasks.Add(task);

        Debug.Log($"接受任务: {task.taskTitle}");
    }

    public void UpdateCollectTaskProgress(string itemId, int amount)
    {
        foreach (var activeTask in ActiveTasks)
        {
            if (activeTask.taskObjectiveType == TaskObjectiveType.CollectItem &&
                activeTask.objectiveTargetId == itemId)
            {
                activeTask.CurrentItemCount += amount;
                CheckTaskCompletion(activeTask); // 传递参数
            }
        }
    }

    public void CompleteLocationTask(string locationId)
    {
        foreach (var activeTask in ActiveTasks)
        {
            if (activeTask.taskObjectiveType == TaskObjectiveType.ReachLocation &&
                activeTask.objectiveTargetId == locationId)
            {
                activeTask.IsLocationReached = true;
                CheckTaskCompletion(activeTask); // 传递参数
            }
        }
    }

    public void CompleteInteractionTask(string objectId)
    {
        foreach (var activeTask in ActiveTasks)
        {
            if (activeTask.taskObjectiveType == TaskObjectiveType.InteractWith &&
                activeTask.objectiveTargetId == objectId)
            {
                activeTask.IsObjectInteracted = true;
                CheckTaskCompletion(activeTask); // 传递参数
            }
        }
    }

    // 在 TaskManager.cs 中
    private void CheckTaskCompletion(Task taskToCheck)
    {
        // 调用重命名后的方法
        taskToCheck.EvaluateCompletion();

        if (taskToCheck.IsTaskCompleted && !CompletedTasks.Contains(taskToCheck))
        {
            CompletedTasks.Add(taskToCheck);
            Debug.Log($"任务完成: {taskToCheck.taskTitle}");
            TaskEvents.NotifyTaskCompleted(taskToCheck);
        }
    }

    public void ClaimTaskReward(Task task)
    {
        if (task == null || !task.IsTaskCompleted || task.IsRewardClaimed) return;

        if (PlayerInventory.Instance != null)
        {
            if (task.taskReward.currencyAmount > 0)
            {
                PlayerInventory.Instance.AddCurrency(task.taskReward.currencyAmount);
            }

            if (!string.IsNullOrEmpty(task.taskReward.rewardItemId))
            {
                PlayerInventory.Instance.AddItem(task.taskReward.rewardItemId);
            }
        }

        task.IsRewardClaimed = true;
        ActiveTasks.Remove(task);
        CompletedTasks.Remove(task);

        Debug.Log($"领取任务奖励: {task.taskTitle}");
    }
}

public static class TaskEvents
{
    public delegate void TaskCompletedHandler(Task task);
    public static event TaskCompletedHandler OnTaskCompleted;

    public static void NotifyTaskCompleted(Task task)
    {
        OnTaskCompleted?.Invoke(task);
    }
}
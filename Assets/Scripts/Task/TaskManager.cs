using System.Collections.Generic;
using UnityEngine;
using Game.Tasks;
using NodeCanvas.Framework; // 添加 NodeCanvas 命名空间 
public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    [Header("TASK SETTINGS")]
    [SerializeField] private List<Task> _allTasks = new List<Task>();

    public List<Task> ActiveTasks { get; private set; } = new List<Task>();
    public List<Task> CompletedTasks { get; private set; } = new List<Task>();
    public List<Task> AllTasks => _allTasks;
    public delegate void FirstMissionAcceptedHandler();
    public static event FirstMissionAcceptedHandler OnFirstMissionAccepted;
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
    public void DebugTasks()
    {
        Debug.Log("=== TASK MANAGER DEBUG ===");
        Debug.Log("All Tasks: " + _allTasks.Count);
        Debug.Log("Active Tasks: " + ActiveTasks.Count);
        Debug.Log("Completed Tasks: " + CompletedTasks.Count);

        foreach (var task in _allTasks)
        {
            Debug.Log($"Task: {task.taskTitle} | Active: {task.IsTaskActive} | Completed: {task.IsTaskCompleted}");
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

        // 检查是否是第一个任务
        if (ActiveTasks.Count == 1)
        {
            // 触发事件，让对话树处理后续逻辑
            OnFirstMissionAccepted?.Invoke();
        }
    }
    public static event FirstMissionAcceptedHandler FirstMissionAccepted;
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
    private void CheckTaskCompletion(Task taskToCheck)
    {
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
    public Task GetTaskById(string taskId)
    {
        return _allTasks.Find(t => t.taskId == taskId);
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
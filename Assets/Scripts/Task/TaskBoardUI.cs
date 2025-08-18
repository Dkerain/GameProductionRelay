using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskBoardUI : MonoBehaviour
{
    public static TaskBoardUI Instance;

    [Header("UI ELEMENTS")]
    public GameObject taskBoardPanel;
    public Transform taskListContainer;
    public GameObject taskEntryPrefab;
    public Text taskTitleText;
    public Text taskDescriptionText;
    public Text taskProgressText;
    public Button acceptButton;
    public Button claimButton;
    public Button closeButton;

    [Header("VISUAL SETTINGS")]
    public Color selectedEntryColor = new Color(0.3f, 0.5f, 0.8f, 0.5f);
    public Color normalEntryColor = new Color(0, 0, 0, 0.3f);

    [Header("ANIMATION")]
    public Animator boardAnimator;
    public float closeAnimationDelay = 0.5f;

    private Task _selectedTask;
    private List<TaskEntryUI> _taskEntries = new List<TaskEntryUI>();
    public bool IsOpen { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 注意：根据项目需要决定是否使用 DontDestroyOnLoad
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        taskBoardPanel.SetActive(false);
        IsOpen = false;

        // 设置按钮事件
        closeButton.onClick.AddListener(CloseTaskBoard);
        acceptButton.onClick.AddListener(AcceptSelectedTask);
        claimButton.onClick.AddListener(ClaimSelectedTask);

        // 注册任务事件
        TaskEvents.OnTaskCompleted += OnTaskCompleted;
    }

    void OnDestroy()
    {
        // 注销事件
        TaskEvents.OnTaskCompleted -= OnTaskCompleted;
    }

    public void OpenTaskBoard()
    {
        if (IsOpen) return;

        taskBoardPanel.SetActive(true);
        IsOpen = true;

        // 播放动画
        if (boardAnimator)
            boardAnimator.Play("BoardOpen");

        PopulateTasks();

        // 自动选择第一个任务
        if (_taskEntries.Count > 0)
        {
            _taskEntries[0].SelectTask();
        }

        // 播放音效
        if (AudioManager.Instance != null && AudioManager.Instance.taskBoardOpen != null)
        {
            AudioManager.Instance.Play(AudioManager.Instance.taskBoardOpen);
        }
    }

    public void CloseTaskBoard()
    {
        if (!IsOpen) return;

        // 播放关闭动画
        if (boardAnimator)
        {
            boardAnimator.Play("BoardClose");
            StartCoroutine(CloseAfterAnimation());
        }
        else
        {
            CompleteClose();
        }
    }

    private IEnumerator CloseAfterAnimation()
    {
        yield return new WaitForSeconds(closeAnimationDelay);
        CompleteClose();
    }

    private void CompleteClose()
    {
        taskBoardPanel.SetActive(false);
        IsOpen = false;

        // 播放音效
        if (AudioManager.Instance != null && AudioManager.Instance.taskBoardClose != null)
        {
            AudioManager.Instance.Play(AudioManager.Instance.taskBoardClose);
        }
    }

    private void PopulateTasks()
    {
        // 清空现有任务条目
        ClearTaskEntries();

        // 获取可用任务
        List<Task> availableTasks = TaskManager.Instance.GetAvailableTasks();

        // 创建新任务条目
        foreach (var task in availableTasks)
        {
            CreateTaskEntry(task);
        }

        // 如果没有任务，显示空状态
        if (availableTasks.Count == 0)
        {
            taskTitleText.text = "无可用任务";
            taskDescriptionText.text = "当前没有可接受的任务，请稍后再来查看。";
            taskProgressText.text = "";
            acceptButton.interactable = false;
            claimButton.interactable = false;
        }
    }

    private void ClearTaskEntries()
    {
        foreach (var entry in _taskEntries)
        {
            if (entry != null && entry.gameObject != null)
                Destroy(entry.gameObject);
        }
        _taskEntries.Clear();
    }

    private void CreateTaskEntry(Task task)
    {
        var entryObj = Instantiate(taskEntryPrefab, taskListContainer);
        var entryUI = entryObj.GetComponent<TaskEntryUI>();

        if (entryUI != null)
        {
            entryUI.Initialize(task, this);
            _taskEntries.Add(entryUI);
        }
    }

    public void SelectTask(Task task)
    {
        // 取消所有条目的选中状态
        foreach (var entry in _taskEntries)
        {
            entry.Deselect();
        }

        // 设置新选中的任务
        _selectedTask = task;

        // 更新任务详情显示
        taskTitleText.text = task.taskTitle;
        taskDescriptionText.text = task.taskDescription;
        taskProgressText.text = $"进度: {task.GetProgressText()}";

        // 更新按钮状态
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        if (_selectedTask == null)
        {
            acceptButton.interactable = false;
            claimButton.interactable = false;
            return;
        }

        // 接受按钮状态
        acceptButton.interactable = !_selectedTask.IsTaskActive;
        acceptButton.GetComponentInChildren<Text>().text =
            _selectedTask.IsTaskActive ? "已接受" : "接受任务";

        // 领取按钮状态
        bool canClaim = _selectedTask.IsTaskCompleted && !_selectedTask.IsRewardClaimed;
        claimButton.interactable = canClaim;
        claimButton.GetComponentInChildren<Text>().text =
            _selectedTask.IsRewardClaimed ? "已领取" : "领取奖励";
    }

    private void AcceptSelectedTask()
    {
        if (_selectedTask != null && !_selectedTask.IsTaskActive)
        {
            TaskManager.Instance.AcceptTask(_selectedTask);
            UpdateButtonStates(); // 刷新按钮状态

            // 播放音效
            if (AudioManager.Instance != null && AudioManager.Instance.taskAccept != null)
            {
                AudioManager.Instance.Play(AudioManager.Instance.taskAccept);
            }
        }
    }

    private void ClaimSelectedTask()
    {
        if (_selectedTask != null && _selectedTask.IsTaskCompleted && !_selectedTask.IsRewardClaimed)
        {
            TaskManager.Instance.ClaimTaskReward(_selectedTask);
            PopulateTasks(); // 刷新任务列表

            // 播放任务完成（领取奖励）音效
            if (AudioManager.Instance != null && AudioManager.Instance.taskComplete != null)
            {
                AudioManager.Instance.Play(AudioManager.Instance.taskComplete);
            }
        }
    }

    // 任务完成时回调
    private void OnTaskCompleted(Task task)
    {
        // 如果任务板是打开的，刷新显示
        if (IsOpen)
        {
            PopulateTasks();

            // 如果完成的是当前选中的任务，更新显示
            if (_selectedTask != null && _selectedTask.taskId == task.taskId)
            {
                SelectTask(task);
            }
        }
    }

    // 外部调用关闭任务板（例如按ESC键）
    public void CloseIfOpen()
    {
        if (IsOpen) CloseTaskBoard();
    }
}
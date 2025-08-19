using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TaskBoardUI : MonoBehaviour
{
    public static TaskBoardUI Instance;

    [Header("UI ELEMENTS")]
    public GameObject taskBoardPanel;
    public Transform taskListContainer;
    public GameObject taskEntryPrefab;
    public GameObject taskDetailPanel; // 确保这个字段在Inspector中正确引用
    public TextMeshProUGUI taskTitleText;
    public TextMeshProUGUI taskDescriptionText;
    public TextMeshProUGUI taskProgressText;
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
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // 确保面板初始状态正确
        if (taskBoardPanel != null)
        {
            taskBoardPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("TaskBoardPanel is not assigned in the inspector!");
        }
        // 确保任务详情面板初始状态正确
        if (taskDetailPanel != null)
        {
            taskDetailPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("TaskDetailPanel is not assigned in the inspector! Attempting to find it...");
            FindTaskDetailPanel();
        }
        IsOpen = false;
        // 设置按钮事件
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseTaskBoard);
        }
        else
        {
            Debug.LogError("Close button reference is missing!");
        }

        // 添加接受按钮事件
        if (acceptButton != null)
        {
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(AcceptSelectedTask);
        }
        else
        {
            Debug.LogError("Accept button reference is missing!");
        }

        // 添加领取按钮事件
        if (claimButton != null)
        {
            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(ClaimSelectedTask);
        }
        else
        {
            Debug.LogError("Claim button reference is missing!");
        }
        // 注册任务事件
        TaskEvents.OnTaskCompleted += OnTaskCompleted;
    }
    private void FindTaskDetailPanel()
    {
        // 尝试在子对象中查找任务详情面板
        if (taskBoardPanel != null)
        {
            // 查找常见的任务详情面板名称
            string[] possibleNames = {
                "TaskDetailPanel", "TaskDetail", "DetailPanel",
                "DetailsPanel", "TaskDetails", "Details"
            };

            foreach (string name in possibleNames)
            {
                Transform detailTransform = taskBoardPanel.transform.Find(name);
                if (detailTransform != null)
                {
                    taskDetailPanel = detailTransform.gameObject;
                    Debug.Log($"Found TaskDetailPanel: {name}");
                    return;
                }
            }
            // 如果没找到，尝试通过组件查找
            var detailPanels = taskBoardPanel.GetComponentsInChildren<Image>(true);
            foreach (var panel in detailPanels)
            {
                if (panel.gameObject != taskBoardPanel &&
                    (panel.gameObject.name.Contains("Detail") || panel.gameObject.name.Contains("Task")))
                {
                    taskDetailPanel = panel.gameObject;
                    Debug.Log($"Found potential TaskDetailPanel: {panel.gameObject.name}");
                    return;
                }
            }
        }

        Debug.LogError("Could not find TaskDetailPanel automatically. Please assign it in the inspector.");
    }
    void OnDestroy()
    {
        // 注销事件
        TaskEvents.OnTaskCompleted -= OnTaskCompleted;
    }
    public void OpenTaskBoard()
    {
        if (IsOpen) return;
        Debug.Log("Opening task board...");
        // 暂停游戏时间
        Time.timeScale = 0f;
        if (taskBoardPanel != null)
        {
            taskBoardPanel.SetActive(true);
            // 确保面板显示在最前面
            taskBoardPanel.transform.SetAsLastSibling();
        }
        // 确保详情面板初始为隐藏状态
        if (taskDetailPanel != null)
        {
            taskDetailPanel.SetActive(false);
        }
        IsOpen = true;
        // 播放动画
        if (boardAnimator != null)
        {
            boardAnimator.Play("BoardOpen");
        }
        // 确保TaskManager已初始化
        if (TaskManager.Instance == null)
        {
            Debug.LogError("TaskManager instance is null! Trying to find it...");
            FindTaskManager();
        }
        PopulateTasks();
        ShowInstructionState();
        // 播放音效
        if (AudioManager.Instance != null && AudioManager.Instance.taskBoardOpen != null)
        {
            AudioManager.Instance.Play(AudioManager.Instance.taskBoardOpen);
        }
    }
    private void ShowInstructionState()
    {
        if (taskDetailPanel != null && !taskDetailPanel.activeSelf)
        {
            taskDetailPanel.SetActive(true);
        }

        if (taskTitleText != null) taskTitleText.text = "选择任务";
        if (taskDescriptionText != null) taskDescriptionText.text = "请从左侧列表中选择一个任务查看详情";
        if (taskProgressText != null) taskProgressText.text = "";
        if (acceptButton != null) acceptButton.interactable = false;
        if (claimButton != null) claimButton.interactable = false;
    }
    private IEnumerator HideInstructionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 如果没有选中任务，隐藏详情面板
        if (_selectedTask == null && taskDetailPanel != null)
        {
            taskDetailPanel.SetActive(false);
        }
    }
    public void SelectTask(Task task)
    {
        if (task == null)
        {
            Debug.LogError("Cannot select null task!");
            return;
        }

        // 取消所有条目的选中状态
        foreach (var entry in _taskEntries)
        {
            entry.Deselect();
        }

        // 设置新选中的任务
        _selectedTask = task;

        // 显示任务详情面板
        if (taskDetailPanel != null)
        {
            taskDetailPanel.SetActive(true);
            Debug.Log($"显示任务详情: {task.taskTitle}");
        }
        else
        {
            Debug.LogError("TaskDetailPanel reference is still null! Cannot display task details.");
            return;
        }

        // 更新任务详情显示
        if (taskTitleText != null) taskTitleText.text = task.taskTitle;
        if (taskDescriptionText != null) taskDescriptionText.text = task.taskDescription;
        if (taskProgressText != null) taskProgressText.text = $"进度: {task.GetProgressText()}";

        // 更新按钮状态
        UpdateButtonStates();
    }
    public void HideTaskDetail()
    {
        if (taskDetailPanel != null)
        {
            taskDetailPanel.SetActive(false);
        }
    }
    public void DeselectTask()
    {
        _selectedTask = null;

        // 隐藏任务详情面板
        HideTaskDetail();

        // 重置按钮状态
        UpdateButtonStates();

        // 显示提示信息
        ShowInstructionState();
    }
    private void FindTaskManager()
    {
        TaskManager manager = FindObjectOfType<TaskManager>();
        if (manager != null)
        {
            Debug.Log("TaskManager found in scene.");
        }
        else
        {
            Debug.LogError("TaskManager not found in scene! Please add a TaskManager object.");
        }
    }
    public void CloseTaskBoard()
    {
        if (!IsOpen) return;

        Debug.Log("Closing task board...");

        // 取消选择当前任务
        DeselectTask();

        // 直接关闭，不依赖动画
        CompleteClose();

        // 如果有动画，也确保它能正确工作
        if (boardAnimator != null && boardAnimator.isActiveAndEnabled)
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
        if (taskBoardPanel != null)
        {
            taskBoardPanel.SetActive(false);
        }
        IsOpen = false;
        // 恢复游戏时间
        Time.timeScale = 1f;
        Debug.Log("Task board closed successfully.");
    }
    private void PopulateTasks()
    {
        // 清空现有任务条目
        ClearTaskEntries();
        // 检查TaskManager实例
        if (TaskManager.Instance == null)
        {
            Debug.LogError("Cannot populate tasks: TaskManager instance is null!");
            ShowErrorState("任务系统未初始化", "无法加载任务数据，请检查游戏设置。");
            return;
        }
        // 获取可用任务
        List<Task> availableTasks = TaskManager.Instance.GetAvailableTasks();
        // 调试信息
        Debug.Log($"Found {availableTasks.Count} available tasks");
        foreach (var task in availableTasks)
        {
            Debug.Log($"Available task: {task.taskTitle} (ID: {task.taskId})");
        }
        // 创建新任务条目
        foreach (var task in availableTasks)
        {
            CreateTaskEntry(task);
        }
        // 如果没有任务，显示空状态
        if (availableTasks.Count == 0)
        {
            ShowEmptyState();
        }
    }
    private void ShowErrorState(string title, string message)
    {
        if (taskTitleText != null) taskTitleText.text = title;
        if (taskDescriptionText != null) taskDescriptionText.text = message;
        if (taskProgressText != null) taskProgressText.text = "";
        if (acceptButton != null) acceptButton.interactable = false;
        if (claimButton != null) claimButton.interactable = false;
    }
    private void ShowEmptyState()
    {
        if (taskTitleText != null) taskTitleText.text = "无可用任务";
        if (taskDescriptionText != null) taskDescriptionText.text = "当前没有可接受的任务，请稍后再来查看。";
        if (taskProgressText != null) taskProgressText.text = "";
        if (acceptButton != null) acceptButton.interactable = false;
        if (claimButton != null) claimButton.interactable = false;
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
        if (taskEntryPrefab == null)
        {
            Debug.LogError("Task entry prefab is not assigned!");
            return;
        }
        if (taskListContainer == null)
        {
            Debug.LogError("Task list container is not assigned!");
            return;
        }
        var entryObj = Instantiate(taskEntryPrefab, taskListContainer);
        var entryUI = entryObj.GetComponent<TaskEntryUI>();
        if (entryUI != null)
        {
            entryUI.Initialize(task, this);
            _taskEntries.Add(entryUI);
            // 调试信息
            Debug.Log($"Created task entry for: {task.taskTitle}");
        }
        else
        {
            Debug.LogError("TaskEntryUI component not found on prefab!");

            // 尝试查找组件
            entryUI = entryObj.GetComponentInChildren<TaskEntryUI>();
            if (entryUI != null)
            {
                entryUI.Initialize(task, this);
                _taskEntries.Add(entryUI);
                Debug.Log("Found TaskEntryUI in children and initialized");
            }
        }
    }
    private void RefreshTaskEntries()
    {
        if (_taskEntries == null || _taskEntries.Count == 0) return;

        // 使用协程确保在Time.timeScale为0时也能更新
        StartCoroutine(RefreshTaskEntriesCoroutine());
    }
    private IEnumerator RefreshTaskEntriesCoroutine()
    {
        // 等待一帧确保UI更新
        yield return null;

        foreach (var entry in _taskEntries)
        {
            if (entry != null)
            {
                // 获取任务的最新状态
                Task task = TaskManager.Instance.GetTaskById(entry.GetTask().taskId);
                if (task != null)
                {
                    // 更新条目的任务引用
                    entry.SetTask(task);
                }
            }
        }
        // 如果当前有选中的任务，也需要更新它的显示
        if (_selectedTask != null)
        {
            Task updatedTask = TaskManager.Instance.GetTaskById(_selectedTask.taskId);
            if (updatedTask != null)
            {
                _selectedTask = updatedTask;
                UpdateButtonStates();
            }
        }
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
        // 修改按钮文本获取方式
        acceptButton.GetComponentInChildren<TextMeshProUGUI>().text =
            _selectedTask.IsTaskActive ? "已接受" : "接受任务";
        // 领取按钮状态
        bool canClaim = _selectedTask.IsTaskCompleted && !_selectedTask.IsRewardClaimed;
        claimButton.interactable = canClaim;
        // 修改按钮文本获取方式
        claimButton.GetComponentInChildren<TextMeshProUGUI>().text =
            _selectedTask.IsRewardClaimed ? "已领取" : "领取奖励";
    }
    private void AcceptSelectedTask()
    {
        if (_selectedTask != null && !_selectedTask.IsTaskActive)
        {
            TaskManager.Instance.AcceptTask(_selectedTask);
            UpdateButtonStates(); // 刷新按钮状态
            // 刷新任务条目显示
            RefreshTaskEntries();
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
            // 刷新任务条目显示
            RefreshTaskEntries();
            // 播放任务完成（领取奖励）音效
            if (AudioManager.Instance != null && AudioManager.Instance.taskComplete != null)
            {
                AudioManager.Instance.Play(AudioManager.Instance.taskComplete);
            }
        }
    }
    private void OnTaskCompleted(Task task)
    {
        // 如果任务板是打开的，刷新显示
        if (IsOpen)
        {
            // 刷新任务条目
            RefreshTaskEntries();
            // 如果完成的是当前选中的任务，更新显示
            if (_selectedTask != null && _selectedTask.taskId == task.taskId)
            {
                Task updatedTask = TaskManager.Instance.GetTaskById(task.taskId);
                if (updatedTask != null)
                {
                    SelectTask(updatedTask);
                }
            }
        }
    }
    // 外部调用关闭任务板（例如按ESC键）
    public void CloseIfOpen()
    {
        if (IsOpen) CloseTaskBoard();
    }
}
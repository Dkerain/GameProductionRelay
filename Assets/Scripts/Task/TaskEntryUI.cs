using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TaskEntryUI : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    public TextMeshProUGUI taskTitleText;
    public Image statusIcon;
    public Button selectButton;
    public Image background;
    public Button deselectButton; // 取消选中按钮

    [Header("STATUS ICONS")]
    public Sprite availableIcon;
    public Sprite activeIcon;
    public Sprite completeIcon;
    private Task _task;
    private TaskBoardUI _taskBoard;
    private bool _isSelected = false;
    public void Initialize(Task task, TaskBoardUI boardUI)
    {
        _task = task;
        _taskBoard = boardUI;
        // 设置任务标题
        if (taskTitleText != null && task != null)
        {
            taskTitleText.text = task.taskTitle;
        }
        // 更新状态图标
        UpdateStatusIcon();
        // 设置选择按钮事件
        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(SelectThisTask);
        }
        // 设置取消选择按钮事件
        if (deselectButton != null)
        {
            deselectButton.onClick.RemoveAllListeners();
            deselectButton.onClick.AddListener(DeselectThisTask);
            deselectButton.gameObject.SetActive(false); // 初始隐藏
        }
        // 初始背景色
        if (background != null)
        {
            background.color = _taskBoard.normalEntryColor;
        }
    }
    // 更新状态图标（确保是public）
    public void UpdateStatusIcon()
    {
        if (statusIcon == null) return;

        if (!_task.IsTaskActive)
        {
            statusIcon.sprite = availableIcon;
            statusIcon.color = Color.yellow;
        }
        else if (_task.IsTaskCompleted)
        {
            statusIcon.sprite = completeIcon;
            statusIcon.color = Color.green;
        }
        else
        {
            statusIcon.sprite = activeIcon;
            statusIcon.color = Color.white;
        }
    }
    public void SelectTask()
    {
        _taskBoard.SelectTask(_task);
        if (background != null)
        {
            background.color = _taskBoard.selectedEntryColor;
        }
        _isSelected = true;

        // 显示取消选择按钮
        if (deselectButton != null)
        {
            deselectButton.gameObject.SetActive(true);
        }
    }
    public void Deselect()
    {
        if (background != null)
        {
            background.color = _taskBoard.normalEntryColor;
        }
        _isSelected = false;
        // 隐藏取消选择按钮
        if (deselectButton != null)
        {
            deselectButton.gameObject.SetActive(false);
        }
    }
    private void SelectThisTask()
    {
        SelectTask();
    }
    private void DeselectThisTask()
    {
        if (_taskBoard != null)
        {
            _taskBoard.DeselectTask();
        }
        Deselect();
    }
    // 添加获取和设置任务的方法
    public Task GetTask()
    {
        return _task;
    }
    public void SetTask(Task newTask)
    {
        _task = newTask;
        // 更新显示
        if (taskTitleText != null && _task != null)
        {
            taskTitleText.text = _task.taskTitle;
        }
        UpdateStatusIcon();
    }
}
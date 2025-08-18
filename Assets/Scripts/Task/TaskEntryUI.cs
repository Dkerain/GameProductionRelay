using UnityEngine;
using UnityEngine.UI;

public class TaskEntryUI : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    public Text taskTitleText;
    public Image statusIcon;
    public Button selectButton;
    public Image background;

    [Header("STATUS ICONS")]
    public Sprite availableIcon;  // 可接受状态（感叹号）
    public Sprite activeIcon;     // 进行中状态（省略号）
    public Sprite completeIcon;   // 可提交状态（问号）

    private Task _task;
    private TaskBoardUI _taskBoard;

    public void Initialize(Task task, TaskBoardUI boardUI)
    {
        _task = task;
        _taskBoard = boardUI;

        // 设置任务标题
        taskTitleText.text = task.taskTitle;

        // 更新状态图标
        UpdateStatusIcon();

        // 设置按钮事件
        selectButton.onClick.AddListener(SelectThisTask);

        // 初始背景色
        background.color = _taskBoard.normalEntryColor;
    }

    private void UpdateStatusIcon()
    {
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
        background.color = _taskBoard.selectedEntryColor;
    }

    public void Deselect()
    {
        background.color = _taskBoard.normalEntryColor;
    }

    private void SelectThisTask()
    {
        SelectTask();
    }
}
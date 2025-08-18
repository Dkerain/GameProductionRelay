using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine; // 添加这行引用

[Category("Task System")]
[Name("Open Task Board")]
public class OpenTaskBoardAction : ActionTask
{
    protected override void OnExecute()
    {
        if (TaskBoardUI.Instance != null)
        {
            TaskBoardUI.Instance.OpenTaskBoard();
            EndAction(true);
        }
        else
        {
            Debug.LogError("TaskBoardUI instance not found!"); // 需要 UnityEngine 命名空间
            EndAction(false);
        }
    }
}
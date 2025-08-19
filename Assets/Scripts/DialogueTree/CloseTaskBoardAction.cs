using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [Category("Task System")]
    public class CloseTaskBoardAction : ActionTask
    {
        protected override void OnExecute()
        {
            if (TaskBoardUI.Instance != null)
            {
                TaskBoardUI.Instance.CloseTaskBoard();
                EndAction(true);
            }
            else
            {
                Debug.LogError("TaskBoardUI instance not found!");
                EndAction(false);
            }
        }
    }
}
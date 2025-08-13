using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class ShowCoinAmountUI : ActionTask
    {
        protected override void OnExecute()
        {
            PacManManager.Instance.ShowCoinAmountUI();
            EndAction(true);
        }
    }
}
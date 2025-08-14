using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LearnBT
{
    [Category("CustomAction")]
    public class ReStartMoving : ActionTask
    {
        protected override void OnExecute()
        {
            base.OnExecute();
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
            EndAction(true);
        }
    }
}
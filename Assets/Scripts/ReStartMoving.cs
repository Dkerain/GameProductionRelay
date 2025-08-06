using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class ReStartMoving : ActionTask  //继承ActionTask
    {
        protected override void OnExecute() //重写OnExecute方法
        {
            base.OnExecute();
            GameObject.FindWithTag("Player").GetComponent<SimpleMoveController>().enabled = true;
            EndAction(true);    //提供返回值
        }
    }
}
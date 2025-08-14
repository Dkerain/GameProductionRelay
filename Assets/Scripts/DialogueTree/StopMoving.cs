using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LearnBT
{
    [Category("CustomAction")]
    public class StopMoving : ActionTask  //继承ActionTask
    {
        protected override void OnExecute() //重写OnExecute方法
        {
            base.OnExecute();
            GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
            EndAction(true);    //提供返回值
        }
    }
}
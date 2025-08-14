using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LearnBT
{
    [Category("CustomAction")]
    public class ResetRotation : ActionTask  //继承ActionTask
    {
        protected override void OnExecute() //重写OnExecute方法
        {
            base.OnExecute();
            // 获取玩家对象及其组件
            GameObject player = GameObject.FindWithTag("Player");
            Animator animator = player.GetComponent<Animator>();
            FieldOfView fieldOfView = player.GetComponent<FieldOfView>();
            // 重置最后的方向
            animator.SetFloat("LastHorizontal", -1f);
            animator.SetFloat("LastVertical", 0f);
            fieldOfView.currentDirection = FieldOfView.Direction.Left;

            EndAction(true);
        }
    }
}
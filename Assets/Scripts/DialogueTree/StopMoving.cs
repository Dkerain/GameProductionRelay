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
            // 获取玩家对象及其组件
            GameObject player = GameObject.FindWithTag("Player");
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            Animator animator = player.GetComponent<Animator>();

            // 停止移动
            rb.velocity = Vector2.zero;
            movement.enabled = false;

            // 重置动画参数，确保停止行走动画
            if (animator != null)
            {
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Speed", 0f);

             
            }
            EndAction(true);    //提供返回值
        }
    }
}
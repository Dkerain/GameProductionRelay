using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LearnBT
{
    [Category("CustomAction")]
    public class StopMoving : ActionTask  //�̳�ActionTask
    {
        protected override void OnExecute() //��дOnExecute����
        {
            base.OnExecute();
            // ��ȡ��Ҷ��������
            GameObject player = GameObject.FindWithTag("Player");
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            Animator animator = player.GetComponent<Animator>();

            // ֹͣ�ƶ�
            rb.velocity = Vector2.zero;
            movement.enabled = false;

            // ���ö���������ȷ��ֹͣ���߶���
            if (animator != null)
            {
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Speed", 0f);

             
            }
            EndAction(true);    //�ṩ����ֵ
        }
    }
}
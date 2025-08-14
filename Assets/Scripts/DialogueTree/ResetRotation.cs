using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LearnBT
{
    [Category("CustomAction")]
    public class ResetRotation : ActionTask  //�̳�ActionTask
    {
        protected override void OnExecute() //��дOnExecute����
        {
            base.OnExecute();
            // ��ȡ��Ҷ��������
            GameObject player = GameObject.FindWithTag("Player");
            Animator animator = player.GetComponent<Animator>();
            FieldOfView fieldOfView = player.GetComponent<FieldOfView>();
            // �������ķ���
            animator.SetFloat("LastHorizontal", -1f);
            animator.SetFloat("LastVertical", 0f);
            fieldOfView.currentDirection = FieldOfView.Direction.Left;

            EndAction(true);
        }
    }
}
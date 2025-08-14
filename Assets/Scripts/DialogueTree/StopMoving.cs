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
            GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
            EndAction(true);    //�ṩ����ֵ
        }
    }
}
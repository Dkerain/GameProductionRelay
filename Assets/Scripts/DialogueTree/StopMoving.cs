using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class StopMoving : ActionTask  //�̳�ActionTask
    {
        protected override void OnExecute() //��дOnExecute����
        {
            base.OnExecute();
            GameObject.FindWithTag("Player").GetComponent<SimpleMoveController>().enabled = false;
            EndAction(true);    //�ṩ����ֵ
        }
    }
}
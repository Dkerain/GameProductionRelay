using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class TestForCustomAction : ActionTask
    {
        protected override void OnExecute()
        {
            Debug.Log("TestForCustomAction 执行成功！");
            EndAction(true); // 这句话非常重要，不要忘记了（否则对话会直接退出，不再执行后面剩余的对话）
        }
    }
}
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class TurnOnTheLight : ActionTask
    {
        protected override void OnExecute()
        {
            // 找到名字为LightManager的GameObject对应的LightManager脚本
            FogOfWarManager fogOfWarManager = GameObject.Find("FogOfWarManager").GetComponent<FogOfWarManager>();
            if (fogOfWarManager != null)
            {
                fogOfWarManager.TurnOnTheLight();
                Debug.Log("Lights have been turned off");
                EndAction(true); // 结束任务并返回成功
            }
            else
            {
                Debug.LogError("LightManager not found!");
                EndAction(false); // 结束任务并返回失败
            }
        }
    }
}
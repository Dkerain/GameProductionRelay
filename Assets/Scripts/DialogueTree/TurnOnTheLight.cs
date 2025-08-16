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
            // �ҵ�����ΪLightManager��GameObject��Ӧ��LightManager�ű�
            FogOfWarManager fogOfWarManager = GameObject.Find("FogOfWarManager").GetComponent<FogOfWarManager>();
            if (fogOfWarManager != null)
            {
                fogOfWarManager.TurnOnTheLight();
                Debug.Log("Lights have been turned off");
                EndAction(true); // �������񲢷��سɹ�
            }
            else
            {
                Debug.LogError("LightManager not found!");
                EndAction(false); // �������񲢷���ʧ��
            }
        }
    }
}
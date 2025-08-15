using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class FadeFromBlack : ActionTask
    {
        [Tooltip("��������ʱ��(��)")]
        public BBParameter<float> fadeDuration = 1f;

        protected override void OnExecute()
        {
            BlackScreenTransition transition = GameObject.FindObjectOfType<BlackScreenTransition>();
            if (transition != null)
            {
                transition.FadeFromBlack(() => EndAction(true));
            }
            else
            {
                Debug.LogError("BlackScreenTransition not found in scene!");
                EndAction(false);
            }
        }
    }
}
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class FullBlackScreenTransition : ActionTask
    {
        [Tooltip("淡入持续时间(秒)")]
        public BBParameter<float> fadeInDuration = 1f;

        [Tooltip("黑屏保持时间(秒)")]
        public BBParameter<float> holdDuration = 1f;

        [Tooltip("淡出持续时间(秒)")]
        public BBParameter<float> fadeOutDuration = 1f;

        protected override void OnExecute()
        {
            BlackScreenTransition transition = GameObject.FindObjectOfType<BlackScreenTransition>();
            if (transition != null)
            {
                // 临时修改持续时间参数
                float originalDuration = transition.fadeDuration;
                transition.fadeDuration = fadeInDuration.value;

                transition.FullTransition(
                    holdDuration.value,
                    () => {
                        // 恢复原始设置
                        transition.fadeDuration = originalDuration;
                        EndAction(true);
                    }
                );
            }
            else
            {
                Debug.LogError("BlackScreenTransition not found in scene!");
                EndAction(false);
            }
        }
    }
}
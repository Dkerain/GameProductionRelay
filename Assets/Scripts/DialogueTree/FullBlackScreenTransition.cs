using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class FullBlackScreenTransition : ActionTask
    {
        [Tooltip("�������ʱ��(��)")]
        public BBParameter<float> fadeInDuration = 1f;

        [Tooltip("��������ʱ��(��)")]
        public BBParameter<float> holdDuration = 1f;

        [Tooltip("��������ʱ��(��)")]
        public BBParameter<float> fadeOutDuration = 1f;

        protected override void OnExecute()
        {
            BlackScreenTransition transition = GameObject.FindObjectOfType<BlackScreenTransition>();
            if (transition != null)
            {
                // ��ʱ�޸ĳ���ʱ�����
                float originalDuration = transition.fadeDuration;
                transition.fadeDuration = fadeInDuration.value;

                transition.FullTransition(
                    holdDuration.value,
                    () => {
                        // �ָ�ԭʼ����
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
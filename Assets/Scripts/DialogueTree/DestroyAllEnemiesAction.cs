using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class DestroyAllEnemiesAction : ActionTask
    {
        protected override void OnExecute()
        {
            // ��ȡ���˹�����ʵ��
            EnemyManager enemyManager = GameObject.Find("EnemyManager")?.GetComponent<EnemyManager>();

            if (enemyManager != null)
            {
                // �������ٷ���
                enemyManager.DestroyAllEnemies();
                Debug.Log("All enemies destroyed");
            }
            else
            {
                Debug.LogWarning("EnemyManager not found!");
            }

            EndAction(true);
        }
    }
}
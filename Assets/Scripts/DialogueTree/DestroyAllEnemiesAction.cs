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
            // 获取敌人管理器实例
            EnemyManager enemyManager = GameObject.Find("EnemyManager")?.GetComponent<EnemyManager>();

            if (enemyManager != null)
            {
                // 调用销毁方法
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
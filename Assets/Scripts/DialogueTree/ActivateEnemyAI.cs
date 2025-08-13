using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class ActivateEnemyAI : ActionTask
    {
        protected override void OnExecute()
        {
            // 找到名字为EnemyManager的GameObject对应的EnemyManager脚本
            EnemyManager enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            enemyManager.StartEnemyAI();
            Debug.Log("Enemies start to move");
            EndAction(true);
        }
    }
}
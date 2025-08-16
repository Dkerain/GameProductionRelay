using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;

namespace LearnBT
{
    [Category("CustomAction")]
    public class ShowEnemy : ActionTask
    {
        protected override void OnExecute()
        {
            // �ҵ�����ΪEnemyManager��GameObject��Ӧ��EnemyManager�ű�
            EnemyManager enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            enemyManager.ShowEnemy();
            Debug.Log("Enemies show");
            EndAction(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public GameObject Enemys; // 子物体为所有敌人

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void StartEnemyAI()
    {
        // 遍历所有敌人，将所有挂载的AIPath脚本的Can Move属性设置为true
        foreach (Transform enemy in Enemys.transform)
        {
            var aiPath = enemy.GetComponent<Pathfinding.AIPath>();
            if (aiPath != null)
            {
                aiPath.canMove = true; // 启用敌人的移动
            }
        }
    }
}

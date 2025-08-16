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
        foreach (Transform enemy in Enemys.transform)
        {
            var aiPath = enemy.GetComponent<Pathfinding.AIPath>();
            if (aiPath != null)
            {
                aiPath.canMove = true;
            }
          
        }
    }

    public void DestroyAllEnemies()
    {
        // 高效版本（从后往前销毁）
        for (int i = Enemys.transform.childCount - 1; i >= 0; i--)
        {
            GameObject enemy = Enemys.transform.GetChild(i).gameObject;
            Destroy(enemy);
        }
    }

    public void ShowEnemy()
    {

        foreach (Transform enemy in Enemys.transform)
        {
           
            enemy.gameObject.SetActive(true);

        }

    }
}
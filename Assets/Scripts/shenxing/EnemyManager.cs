using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public GameObject Enemys; // 子物体为所有敌人

    [Header("追逐音乐")] public AudioClip RunSound;
    private AudioSource audioSource;

    void Start()
    {
        // 添加或获取AudioSource组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

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
        audioSource.PlayOneShot(RunSound);
    }

    public void DestroyAllEnemies()
    {
        // 高效版本（从后往前销毁）
        for (int i = Enemys.transform.childCount - 1; i >= 0; i--)
        {
            GameObject enemy = Enemys.transform.GetChild(i).gameObject;
            Destroy(enemy);
        }
        audioSource.Stop();
    }

    public void ShowEnemy()
    {

        foreach (Transform enemy in Enemys.transform)
        {
           
            enemy.gameObject.SetActive(true);

        }

    }
}
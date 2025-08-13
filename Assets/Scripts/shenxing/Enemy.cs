using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // 敌人移动速度
    
    private AIPath aiPath; // A*寻路组件

    private void Start()
    {
        aiPath = GetComponent<AIPath>(); // 获取寻路组件
        aiPath.maxSpeed = speed; // 设置敌人的速度
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("敌人碰到玩家了！");
            // 这里可以添加敌人碰到玩家时的逻辑
        }
    }
}

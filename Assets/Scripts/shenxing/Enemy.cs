using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using Pathfinding;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // 敌人移动速度
    
    private AIPath aiPath; // A*寻路组件

    public DialogueTreeController treeController;

    public Blackboard GlobalBlackboard;

    private void Start()
    {
        aiPath = GetComponent<AIPath>(); // 获取寻路组件
        aiPath.maxSpeed = speed; // 设置敌人的速度
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {


       if (other.CompareTag("Player"))
        {

    
            Debug.Log("怪物碰到玩家了！");

            GlobalBlackboard.SetVariableValue("WinTheRunGame", false);
            GlobalBlackboard.SetVariableValue("OverTheRunGame", true);
            // 找到名字为EnemyManager的GameObject对应的EnemyManager脚本
            EnemyManager enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

            enemyManager.DestroyAllEnemies();
            PacManManager.Instance.HideCoins();
            Debug.Log("Enemies die");

            treeController.StartDialogue();
       
        }
        


    }
}

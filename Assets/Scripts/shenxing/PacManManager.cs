// PacManManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NodeCanvas.Framework;
using NodeCanvas.DialogueTrees;

/// <summary>
/// 吃豆人管理器
/// 吃完所有金币后的具体逻辑还没有写，目前只是有一个文字的提示
/// </summary>
public class PacManManager : MonoBehaviour
{
    public static PacManManager Instance;
    public Blackboard GlobalBalckboard;
    public DialogueTreeController dialogueTreeController;

    
    [Header("UI Settings")]
    public TextMeshProUGUI coinCounterText;
    public GameObject RunGamePanel;

    [Header("Coins")]
    [SerializeField] private GameObject coins;
    
    [SerializeField] private int totalCoins;
    [SerializeField] public int collectedCoins;

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

    void Start()
    {
        FindAllCoins();
        UpdateCoinUI();
        HideCoins();
    }

    // 场景初始化时查找所有金币
    void FindAllCoins()
    {
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        collectedCoins = 0;
    }

    // 收集金币时调用
    public void CollectCoin()
    {
        collectedCoins++;
        UpdateCoinUI();
        
        // 检查游戏胜利条件
        if (collectedCoins >= totalCoins)
        {
            Debug.Log("所有金币收集完成！");
            // 这里可以添加游戏胜利逻辑
            GlobalBalckboard.SetVariableValue("OverTheRunGame", true);
            GlobalBalckboard.SetVariableValue("WinTheRunGame", true);

            dialogueTreeController.StartDialogue();
            // 找到名字为EnemyManager的GameObject对应的EnemyManager脚本
            EnemyManager enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

            enemyManager.DestroyAllEnemies();
            HideCoins();
            Debug.Log("Enemies die");

        }
    }

    // 更新UI显示
    void UpdateCoinUI()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = $"Coin: {collectedCoins}/{totalCoins}";
        }
    }
    
    // 显示金币数量UI
    public void ShowCoinAmountUI()
    {
        if (coinCounterText != null && RunGamePanel != null)
        {
            RunGamePanel.SetActive(true);
        }
    }
    
    // 显示金币
    public void ShowCoins()
    {
        if (coins == null)
        {
            Debug.LogError("Coins GameObject is not assigned in PacManManager.");
            return;
        }
        
        // 遍历coins的所有子物体，并启用它们
        foreach (Transform coin in coins.transform)
        {
            coin.gameObject.SetActive(true);
        }
    }
    
    public void HideCoins()
    {
        if (coins == null)
        {
            Debug.LogError("Coins GameObject is not assigned in PacManManager.");
            return;
        }
        
        // 遍历coins的所有子物体，并禁用它们
        foreach (Transform coin in coins.transform)
        {
            coin.gameObject.SetActive(false);
        }

        //关闭coinsUI
        if (coinCounterText != null && RunGamePanel != null)
        {
            RunGamePanel.SetActive(false);
        }
    }
}
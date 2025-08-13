// PacManManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PacManManager : MonoBehaviour
{
    public static PacManManager Instance;
    
    [Header("UI Settings")]
    public TextMeshProUGUI coinCounterText;
    
    private int totalCoins;
    private int collectedCoins;

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
}
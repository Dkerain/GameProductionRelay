using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    public static GM Instance;

    [Header("游戏时间设置")]
    public float gameDuration = 60f; // 总游戏时间（秒）
    private float currentTime;
    public bool isGameRunning = false;

    [Header("分数设置")]
    public int score = 0;

    [Header("UI组件")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public Text finalScoreText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ShowStartPanel();
    }

    void Update()
    {
        if (isGameRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();

            if (currentTime <= 0f)
            {
                EndGame();
            }
        }
    }

    public void StartGame()
    {
        score = 0;
        currentTime = gameDuration;
        isGameRunning = true;

        UpdateScoreUI();
        UpdateTimerUI();

        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void EndGame()
    {
        Debug.Log("Game Over!");
        isGameRunning = false;
        gameOverPanel.SetActive(true);
        // finalScoreText.text = "得分：" + score;
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "得分：" + score;
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = "时间：" + Mathf.CeilToInt(currentTime);
    }

    private void ShowStartPanel()
    {
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }
    
    [ContextMenu("RestartGame")]
    // 可绑定给“重新开始”按钮
    public void RestartGame()
    {
        StartGame();
    }
}
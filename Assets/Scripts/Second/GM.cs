using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    public static GM Instance;

    public PacManManager pacManManager;

    [Header("游戏时间设置")]
    public float gameDuration = 60f; // 总游戏时间（秒）
    private float currentTime;
    public bool isGameRunning = false;

    [Header("分数设置")]
    public int score = 0;
    public int targetScore = 24;

    [Header("UI组件")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject gamePanel;
    public Button startButton;
    public Button exitButton;
    public GameObject gameOverMemberPanel;
    [Header("整个游戏开始按钮")]public Button firstStartGame;
    [Header("整个游戏开始面板")] public GameObject firstStartPanel;

    [Header("演职人员表设置")]
    public TextMeshProUGUI creditsText;
    public float scrollSpeed = 30f;
    public float creditsDuration = 10f;
    public Button endGameButton;

    [Header("nodeCanvas")]
    public DialogueTreeController FinalTreeController;
    public DialogueTreeController FirstTreeController;
    public Blackboard globalBlackboard;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        if (startButton != null)
            startButton.onClick.AddListener(StartGame);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitHitGame);

        if (firstStartGame != null)
            firstStartGame.onClick.AddListener(StartGameFirst);
    }

    void Update()
    {
        if (isGameRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();

            if (currentTime <= 0f || score >= targetScore)
            {
                EndGame();
            }
        }
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);

        score = pacManManager.collectedCoins;//分数继承至上一个游戏的
        currentTime = gameDuration;
        isGameRunning = true;

        UpdateScoreUI();
        UpdateTimerUI();
    }

    public void EndGame()
    {
        Debug.Log("Game Over!");
        isGameRunning = false;
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        if(score >= targetScore)
        {
            globalBlackboard.SetVariableValue("WinTheHItGame", true);
        }
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
   

    public void EnterHitGame()
    {
        startPanel.SetActive(true);
    }

    public void ExitHitGame() 
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        FinalTreeController.StartDialogue();//尾声
    }

    [ContextMenu("测试结束名单")]
    public void GameOverShowMember()
    {
        gameOverMemberPanel.SetActive(true);
        StartCoroutine(ScrollCredits());
    }

    private IEnumerator ScrollCredits()
    {
        // 重置文本位置
        RectTransform textRect = creditsText.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(0, -Screen.height);

        // 开始滚动
        float timer = 0f;
        while (timer < creditsDuration)
        {
            timer += Time.deltaTime;
            // 向上滚动文本
            textRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            yield return null;
        }

        // 滚动结束后显示结束按钮
        if (endGameButton != null)
        {
            endGameButton.gameObject.SetActive(true);
            endGameButton.onClick.AddListener(ExitHoleGame);
        }
    }

    public void ExitHoleGame()
    {
        Application.Quit();
    }

    public void StartGameFirst()
    {
        firstStartPanel.SetActive(false);
        FirstTreeController.StartDialogue();
    }

}
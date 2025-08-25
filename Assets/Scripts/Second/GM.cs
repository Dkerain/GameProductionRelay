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
    public float ScrollY = 100f;
    public Button endGameButton;
    [Header("尾声音乐")] public AudioClip FinalSound;
    private AudioSource audioSource;

    [Header("玩家控制")]
    public PlayerMovement playerMovement; // 引用玩家移动脚本


    [Header("nodeCanvas")]
    public DialogueTreeController FinalTreeController;
    public DialogueTreeController FirstTreeController;
    public Blackboard globalBlackboard;

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
            Instance = this;
        else
            Destroy(gameObject);

        if(firstStartPanel != null)
            firstStartPanel.SetActive(true);

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
        ForcePlayerFaceDown();
    }
    private void ForcePlayerFaceDown()
    {
        if (playerMovement != null)
        {
            // 获取玩家的Animator组件
            Animator animator = playerMovement.GetComponent<Animator>();
            if (animator != null)
            {
                // 设置动画参数，强制面向下方
                animator.SetFloat("LastHorizontal", 0f);
                animator.SetFloat("LastVertical", -1f);
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", -1f);

                Debug.Log("强制玩家面向屏幕下方");
            }

            // 如果有FieldOfView组件，也更新方向
            FieldOfView fieldOfView = playerMovement.GetComponent<FieldOfView>();
            if (fieldOfView != null)
            {
                fieldOfView.currentDirection = FieldOfView.Direction.Down;
            }
        }
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
        endGameButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(FinalSound);
        StartCoroutine(ScrollCredits());
    }

    private IEnumerator ScrollCredits()
    {
        // 重置文本位置
        RectTransform textRect = creditsText.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(0, -Screen.height + ScrollY);

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
        Debug.Log("退出整个游戏");
    }

    public void StartGameFirst()
    {
        firstStartPanel.SetActive(false);
        FirstTreeController.StartDialogue();
    }

}
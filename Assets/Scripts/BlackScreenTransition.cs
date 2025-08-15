using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlackScreenTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] public float fadeDuration = 1.0f; // 黑屏淡入淡出时间
    [SerializeField] private bool startTransparent = true; // 启动时是否透明

    private Image blackScreen;
    private CanvasGroup canvasGroup;
    private Coroutine currentTransition;

    void Awake()
    {
        // 确保黑屏覆盖整个屏幕
        CreateBlackScreen();

        if (startTransparent)
        {
            SetAlpha(0f);
        }
        else
        {
            SetAlpha(1f);
        }
    }

    private void CreateBlackScreen()
    {
        // 创建Canvas
        GameObject canvasGO = new GameObject("BlackScreenCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10; 

        // 创建Image组件
        GameObject imageGO = new GameObject("BlackScreen");
        imageGO.transform.SetParent(canvasGO.transform);
        blackScreen = imageGO.AddComponent<Image>();
        blackScreen.color = Color.black;

        // 设置全屏尺寸
        RectTransform rect = imageGO.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // 添加CanvasGroup用于透明度控制
        canvasGroup = imageGO.AddComponent<CanvasGroup>();
    }

    /// <summary>
    /// 淡入黑屏
    /// </summary>
    /// <param name="onComplete">完成后回调</param>
    public void FadeToBlack(System.Action onComplete = null)
    {
        StartTransition(1f, fadeDuration, onComplete);
    }

    /// <summary>
    /// 淡出黑屏
    /// </summary>
    /// <param name="onComplete">完成后回调</param>
    public void FadeFromBlack(System.Action onComplete = null)
    {
        StartTransition(0f, fadeDuration, onComplete);
    }

    /// <summary>
    /// 完整黑屏过渡序列（淡入->保持->淡出）
    /// </summary>
    /// <param name="holdDuration">黑屏保持时间</param>
    /// <param name="onComplete">完成后回调</param>
    public void FullTransition(float holdDuration, System.Action onComplete = null)
    {
        if (currentTransition != null) StopCoroutine(currentTransition);
        currentTransition = StartCoroutine(FullTransitionRoutine(holdDuration, onComplete));
    }

    private IEnumerator FullTransitionRoutine(float holdDuration, System.Action onComplete)
    {
        yield return StartCoroutine(FadeRoutine(1f, fadeDuration)); // 淡入
        yield return new WaitForSeconds(holdDuration);               // 保持
        yield return StartCoroutine(FadeRoutine(0f, fadeDuration)); // 淡出
        onComplete?.Invoke();
    }

    private void StartTransition(float targetAlpha, float duration, System.Action onComplete)
    {
        if (currentTransition != null) StopCoroutine(currentTransition);
        currentTransition = StartCoroutine(FadeRoutine(targetAlpha, duration, onComplete));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration, System.Action onComplete = null)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, t));
            yield return null;
        }

        SetAlpha(targetAlpha);
        onComplete?.Invoke();
    }

    private void SetAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
        canvasGroup.blocksRaycasts = alpha > 0.01f; // 当可见时阻止点击穿透
    }

    // 可选：直接在编辑器中调用测试
    [ContextMenu("Test Fade To Black")]
    private void TestFadeIn() => FadeToBlack(() => Debug.Log("Fade to black complete"));

    [ContextMenu("Test Fade From Black")]
    private void TestFadeOut() => FadeFromBlack(() => Debug.Log("Fade from black complete"));
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlackScreenTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] public float fadeDuration = 1.0f; // �������뵭��ʱ��
    [SerializeField] private bool startTransparent = true; // ����ʱ�Ƿ�͸��

    private Image blackScreen;
    private CanvasGroup canvasGroup;
    private Coroutine currentTransition;

    void Awake()
    {
        // ȷ����������������Ļ
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
        // ����Canvas
        GameObject canvasGO = new GameObject("BlackScreenCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10; 

        // ����Image���
        GameObject imageGO = new GameObject("BlackScreen");
        imageGO.transform.SetParent(canvasGO.transform);
        blackScreen = imageGO.AddComponent<Image>();
        blackScreen.color = Color.black;

        // ����ȫ���ߴ�
        RectTransform rect = imageGO.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // ���CanvasGroup����͸���ȿ���
        canvasGroup = imageGO.AddComponent<CanvasGroup>();
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="onComplete">��ɺ�ص�</param>
    public void FadeToBlack(System.Action onComplete = null)
    {
        StartTransition(1f, fadeDuration, onComplete);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="onComplete">��ɺ�ص�</param>
    public void FadeFromBlack(System.Action onComplete = null)
    {
        StartTransition(0f, fadeDuration, onComplete);
    }

    /// <summary>
    /// ���������������У�����->����->������
    /// </summary>
    /// <param name="holdDuration">��������ʱ��</param>
    /// <param name="onComplete">��ɺ�ص�</param>
    public void FullTransition(float holdDuration, System.Action onComplete = null)
    {
        if (currentTransition != null) StopCoroutine(currentTransition);
        currentTransition = StartCoroutine(FullTransitionRoutine(holdDuration, onComplete));
    }

    private IEnumerator FullTransitionRoutine(float holdDuration, System.Action onComplete)
    {
        yield return StartCoroutine(FadeRoutine(1f, fadeDuration)); // ����
        yield return new WaitForSeconds(holdDuration);               // ����
        yield return StartCoroutine(FadeRoutine(0f, fadeDuration)); // ����
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
        canvasGroup.blocksRaycasts = alpha > 0.01f; // ���ɼ�ʱ��ֹ�����͸
    }

    // ��ѡ��ֱ���ڱ༭���е��ò���
    [ContextMenu("Test Fade To Black")]
    private void TestFadeIn() => FadeToBlack(() => Debug.Log("Fade to black complete"));

    [ContextMenu("Test Fade From Black")]
    private void TestFadeOut() => FadeFromBlack(() => Debug.Log("Fade from black complete"));
}
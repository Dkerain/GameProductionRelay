using UnityEngine;
using DG.Tweening;

public class LievPopup : MonoBehaviour
{
    [Header("弹出参数")]
    public float popupHeight = 50f;     // 向上弹出的高度
    public float popupDuration = 3f;   // 动画时间
    public Ease easing = Ease.OutBack; // 缓动：快起稳停

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        transform.position = new Vector3(startPos.x, startPos.y - popupHeight, startPos.z);
    }
    [ContextMenu("Popup")]
    public void Popup()
    {
        // 延时弹出（可选），例如 0.1 秒后出现
        Invoke(nameof(DoPopup), 0.1f);
    }
    
    
    public void DoPopup()
    {
        transform.DOMoveY(startPos.y, popupDuration).SetEase(easing);
    }
}
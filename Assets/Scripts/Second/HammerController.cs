using UnityEngine;
using DG.Tweening;

public interface IHittable
{
    bool HasBeenHit { get; }
    void OnHit();
}

public class HammerController : MonoBehaviour
{
    [Header("跟随参数")] public float followSpeed = 20f;
    public Vector3 offset = new Vector3(-0.5f, 0.5f, 0f);

    [Header("打击动画参数")] public float hitRotation = -45f;
    public float hitDuration = 0.05f;
    public float returnDuration = 0.1f;
    public Ease hitEase = Ease.OutQuad;
    public Ease returnEase = Ease.OutBack;


    private bool isHitting = false;

    void Update()
    {
        FollowMouse();

        if (Input.GetMouseButtonDown(0) && !isHitting)
        {
            DoHitAnimation();
        }
    }

    void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = Vector3.Lerp(transform.position, mousePos + offset, Time.deltaTime * followSpeed);
    }

    void DoHitAnimation()
    {
        isHitting = true;

        // 启用打击检测
        GetComponentInChildren<HammerTrigger>().EnableHit();

        // 动画流程
        transform.DORotate(new Vector3(0, 0, hitRotation), hitDuration)
            .SetEase(hitEase)
            .OnComplete(() =>
            {
                transform.DORotate(Vector3.zero, returnDuration)
                    .SetEase(returnEase)
                    .OnComplete(() =>
                    {
                        // 禁用打击检测
                        GetComponentInChildren<HammerTrigger>().DisableHit();
                        isHitting = false;
                    });
            });
    }
}
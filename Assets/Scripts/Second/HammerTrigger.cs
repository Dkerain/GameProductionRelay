using UnityEngine;

public class HammerTrigger : MonoBehaviour
{
    public bool canHit = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (!canHit) return;

        Debug.Log("Hit " + other.name);
        IHittable target = other.GetComponent<IHittable>();
        if (target != null && !target.HasBeenHit)
        {
            target.OnHit();
            canHit = false; // 防止多次判定
        }
    }

    public void EnableHit()
    {
        canHit = true;
    }

    public void DisableHit()
    {
        canHit = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Leaf,
    Alice,
}

public class Character : MonoBehaviour, IHittable
{
    public bool hasBeenHit = false;
    public bool HasBeenHit { get { return hasBeenHit; } }

    public virtual void OnHit()
    {
        hasBeenHit = true;

        // 根据Tag判断好坏并加分减分
        if (gameObject.CompareTag("Bug"))
        {
            GM.Instance.AddScore(1);
            Debug.Log("击中Bug，+1分");
        }
        else if (gameObject.CompareTag("Alice"))
        {
            GM.Instance.AddScore(-1);
            Debug.Log("击中Alice，-1分");
        }

        // 销毁对象
        Destroy(gameObject);
    }

    // 添加自动销毁方法
    public void AutoDestroy()
    {
        if (!hasBeenHit)
        {
            Destroy(gameObject);
        }
    }
}
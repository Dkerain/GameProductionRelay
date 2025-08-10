using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于替换原有的Interable
/// </summary>
public abstract class BaseInteractable : MonoBehaviour
{
    [Header("UI Settings")]
    public InteractUI interactionUI; // 拖入UI对象（最好是Canvas下的UI元素）
    public Vector3 worldOffset = new Vector3(0, 1f, 0); // 世界坐标偏移，控制UI在物体上方的位置

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.F; // 交互按键

    void Start()
    {
        // 初始化组件
        if (!interactionUI)
        {
            interactionUI = Object.FindObjectOfType<InteractUI>();

        }
        if (!interactionUI)
        {
            Debug.LogError("未分配UI对象！", this);
        }

        OnStart();
    }

    protected virtual void OnStart() { }
    
    protected abstract void OnInteract();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionUI.AddInteract(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionUI.RemoveInteract(this);
        }
    }

    public void Interact()
    {
        Debug.Log("与 " + gameObject.name + " 交互");
        OnInteract();
    }

    // 在Scene视图中可视化触发范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + worldOffset, 0.2f);
    }
}
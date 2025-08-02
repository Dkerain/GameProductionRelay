using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject interactionUI; // 拖入UI对象（最好是Canvas下的UI元素）
    public Vector3 worldOffset = new Vector3(0, 1f, 0); // 世界坐标偏移，控制UI在物体上方的位置

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.F; // 交互按键
    public bool showUIOnlyWhenNear = true; // 是否只在靠近时显示UI

    private Camera mainCamera;
    private RectTransform uiRectTransform;
    private bool isPlayerInRange = false;

    public DialogueTreeController dialogueTree;//新建对话树

    void Start()
    {
        // 初始化组件
        mainCamera = Camera.main;
        if (interactionUI != null)
        {
            uiRectTransform = interactionUI.GetComponent<RectTransform>();
            interactionUI.SetActive(false);
        }
        else
        {
            Debug.LogError("未分配UI对象！", this);
        }
    }

    void Update()
    {
        // 如果UI处于激活状态，更新其位置
        if (interactionUI != null && interactionUI.activeSelf)
        {
            UpdateUIPosition();
        }

        // 检测交互输入
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            OnInteract();
        }
    }

    void UpdateUIPosition()
    {
        // 将物体世界坐标转换为屏幕坐标
        Vector3 worldPosition = transform.position + worldOffset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // 更新UI位置
        uiRectTransform.position = screenPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (showUIOnlyWhenNear && interactionUI != null)
            {
                interactionUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }
        }
    }

    // 交互逻辑（可重写）
    protected virtual void OnInteract()
    {
        Debug.Log("与 " + gameObject.name + " 交互");
        // 在这里添加具体交互逻辑
        Destroy(gameObject); // 立即销毁
        // 如果有独立UI，也销毁
        if (interactionUI != null)
        {
            Destroy(interactionUI);
        }
        dialogueTree.StartDialogue();//开启对话树的脚本
    }

    // 在Scene视图中可视化触发范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + worldOffset, 0.2f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public GameObject interactionUI; // 拖拽UI对象到这个字段
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 确保主角有"Player"标签
        {
            interactionUI.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionUI.SetActive(false);
        }
    }
    private void Update()
    {
        if (interactionUI.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            // 执行交互逻辑
            Debug.Log("与对象交互");
        }
    }
}

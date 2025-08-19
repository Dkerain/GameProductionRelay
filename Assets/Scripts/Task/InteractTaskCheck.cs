using UnityEngine;

[RequireComponent(typeof(Collider2D))] // 要求2D碰撞体
public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string _objectId; // 在Inspector中设置的唯一ID
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检测玩家交互
        if (other.CompareTag("Player"))
        {
            // 通知任务管理器完成交互
            TaskManager.Instance?.CompleteInteractionTask(_objectId);
            
            // 可选：禁用碰撞体防止重复触发
            GetComponent<Collider2D>().enabled = false;
            
            Debug.Log($"交互完成: {_objectId}");
        }
    }
}
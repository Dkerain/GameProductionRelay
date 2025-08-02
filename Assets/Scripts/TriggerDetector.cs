using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public GameObject interactionUI; // ��קUI��������ֶ�
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ȷ��������"Player"��ǩ
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
            // ִ�н����߼�
            Debug.Log("����󽻻�");
        }
    }
}

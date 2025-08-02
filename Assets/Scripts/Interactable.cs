using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject interactionUI; // ����UI���������Canvas�µ�UIԪ�أ�
    public Vector3 worldOffset = new Vector3(0, 1f, 0); // ��������ƫ�ƣ�����UI�������Ϸ���λ��

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.F; // ��������
    public bool showUIOnlyWhenNear = true; // �Ƿ�ֻ�ڿ���ʱ��ʾUI

    private Camera mainCamera;
    private RectTransform uiRectTransform;
    private bool isPlayerInRange = false;

    public DialogueTreeController dialogueTree;//�½��Ի���

    void Start()
    {
        // ��ʼ�����
        mainCamera = Camera.main;
        if (interactionUI != null)
        {
            uiRectTransform = interactionUI.GetComponent<RectTransform>();
            interactionUI.SetActive(false);
        }
        else
        {
            Debug.LogError("δ����UI����", this);
        }
    }

    void Update()
    {
        // ���UI���ڼ���״̬��������λ��
        if (interactionUI != null && interactionUI.activeSelf)
        {
            UpdateUIPosition();
        }

        // ��⽻������
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            OnInteract();
        }
    }

    void UpdateUIPosition()
    {
        // ��������������ת��Ϊ��Ļ����
        Vector3 worldPosition = transform.position + worldOffset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // ����UIλ��
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

    // �����߼�������д��
    protected virtual void OnInteract()
    {
        Debug.Log("�� " + gameObject.name + " ����");
        // ��������Ӿ��彻���߼�
        Destroy(gameObject); // ��������
        // ����ж���UI��Ҳ����
        if (interactionUI != null)
        {
            Destroy(interactionUI);
        }
        dialogueTree.StartDialogue();//�����Ի����Ľű�
    }

    // ��Scene��ͼ�п��ӻ�������Χ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + worldOffset, 0.2f);
    }
}

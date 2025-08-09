using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : BaseInteractable
{
    public DialogueTreeController dialogueTree;
    public bool destroyOnEnd;

    protected override void OnInteract()
    {
        print("开始对话");
        dialogueTree.StartDialogue();//开启对话树的脚本
        if (destroyOnEnd)
        {
            Destroy(gameObject); // 立即销毁
            interactionUI.SetActive(false);//替代销毁的，改成停用
        }
    }
}
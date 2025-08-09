using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : BaseInteractable
{
    public DialogueTreeController dialogueTree;

    protected override void OnInteract()
    {
        dialogueTree.StartDialogue();//开启对话树的脚本
        print("开始对话");
        Destroy(gameObject); // 立即销毁
        interactionUI.SetActive(false);//替代销毁的，改成停用
    }
}
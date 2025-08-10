using NodeCanvas.DialogueTrees;
using UnityEngine;

public class AutoTriggerWhenEnter : MonoBehaviour
{
    public DialogueTreeController dialogueTree;
    public bool destroyOnEnd;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueTree.StartDialogue();
            if (destroyOnEnd)
            {
                Destroy(gameObject);
            }
        }
    }
}

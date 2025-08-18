using NodeCanvas.DialogueTrees;
using UnityEngine;

public class TaskBoardInteraction : MonoBehaviour
{
    public DialogueTreeController dialogueTree;
    public DialogueActor taskBoardActor;
    public float interactionRadius = 1.5f;
    public KeyCode interactKey = KeyCode.E;
    public GameObject interactPrompt;

    private bool isPlayerNearby;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (interactPrompt) interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        isPlayerNearby = distance <= interactionRadius;

        if (interactPrompt)
            interactPrompt.SetActive(isPlayerNearby);

        if (isPlayerNearby && Input.GetKeyDown(interactKey))
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if (dialogueTree == null || taskBoardActor == null)
        {
            Debug.LogError("Dialogue references not set!");
            return;
        }

        dialogueTree.StartDialogue(taskBoardActor);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
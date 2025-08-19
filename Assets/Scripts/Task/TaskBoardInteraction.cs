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

    public TaskBoardUI taskBoardUI;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (interactPrompt) interactPrompt.SetActive(false);

        // 如果没有直接引用，尝试查找
        if (taskBoardUI == null)
        {
            taskBoardUI = FindObjectOfType<TaskBoardUI>();
            if (taskBoardUI == null)
            {
                Debug.LogError("TaskBoardUI not found in scene!");
            }
        }
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

        dialogueTree.StartDialogue(taskBoardActor, OnDialogueFinished);
    }
    private void OnDialogueFinished(bool success)
    {
        if (success && taskBoardUI != null)
        {
            // 对话完成后打开任务板
            taskBoardUI.OpenTaskBoard();
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

}
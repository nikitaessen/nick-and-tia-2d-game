using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public string[] dialogueLines;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            dialogueManager.lines = dialogueLines;
            dialogueManager.gameObject.SetActive(true);
            dialogueManager.StartDialogue();
            Destroy(gameObject);
        }
        
    }
}
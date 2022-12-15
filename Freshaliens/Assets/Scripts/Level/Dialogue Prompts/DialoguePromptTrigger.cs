using UnityEngine;
using Freshaliens.Management;

[RequireComponent(typeof(BoxCollider2D))]
public class DialoguePromptTrigger : MonoBehaviour
{
    [SerializeField] private DialoguePromptData dialoguePrompt;
    [SerializeField] private bool overwriteExistingDialogue = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager.Instance.StartDialogue(dialoguePrompt);
        gameObject.SetActive(false);
    }
}

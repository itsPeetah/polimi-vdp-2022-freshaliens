using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DialoguePromptTrigger : MonoBehaviour
{
    [SerializeField] private DialoguePromptData dialoguePrompt;
    [SerializeField] private bool overwriteExistingDialogue = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DialoguePromptDisplayer.Instance.DisplayDialoguePrompt(dialoguePrompt, overwriteExistingDialogue);
    }
}

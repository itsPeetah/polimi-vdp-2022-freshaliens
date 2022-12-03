using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePromptDisplayer : MonoBehaviour
{
    private static DialoguePromptDisplayer instance;
    public static DialoguePromptDisplayer Instance => instance; 

    [Header("Settings")]
    [SerializeField] private float characterTypeTime = 0.05f;
    [SerializeField] private float lineDelayTime = 0.1f;

    [Header("Components")]
    [SerializeField] private GameObject root = null;
    [SerializeField] private TextMeshProUGUI dialogueText = null;
    [SerializeField] private Image faceImage = null;

    // State
    private bool isDisplayingText = false;

    // Properties
    private bool IsDisplayingText { set { isDisplayingText = value; root.SetActive(value); } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        IsDisplayingText = false;
        
    }

    public void DisplayDialoguePrompt(DialoguePromptData dialogue, bool overwriteExistingDialogue = false) {
        faceImage.sprite = dialogue.Face;
        bool canDisplay = !isDisplayingText || overwriteExistingDialogue;
        if (canDisplay)
        {
            StopCoroutine(nameof(TypeDialogue));
            StartCoroutine(nameof(TypeDialogue), dialogue);
        }
    }

    private IEnumerator TypeDialogue(DialoguePromptData dialogue) {

        IsDisplayingText = true;
        
        string[] lines = dialogue.DialogueLines;
        for (int i = 0; i < lines.Length; i++) {

            // Display line
            string textLineSoFar = "";
            string line = lines[i];
        
            for (int j = 0; j < line.Length; j++) {

                // Display text
                textLineSoFar += line[j];
                dialogueText.SetText(textLineSoFar);

                yield return new WaitForSeconds(characterTypeTime);
            }

            yield return new WaitForSeconds(lineDelayTime);
            dialogueText.SetText("");
        }

        IsDisplayingText = false;
    }


}

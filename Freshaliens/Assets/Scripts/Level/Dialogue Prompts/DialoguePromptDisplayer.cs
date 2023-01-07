using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Freshaliens.Player.Components;
using Freshaliens.Management;
namespace Freshaliens.UI
{
    public class DialoguePromptDisplayer : UIScreen
    {
        private static DialoguePromptDisplayer instance;
        public static DialoguePromptDisplayer Instance => instance;

        [Header("Settings")]
        [SerializeField] private float characterTypeTime = 0.05f;
        [SerializeField] private float lineDelayTime = 0.1f;

        [Header("Components")]
        [SerializeField] private TextMeshProUGUI dialogueText = null;
        [SerializeField] private Image faceImage = null;

        // State
        private bool isDisplayingText = false;
        private bool skipTextQueued = false;

        // Properties
        private bool IsDisplayingText { set { isDisplayingText = value; root.SetActive(value); } }

        private void Awake()
        {
            instance = this;
        }

        protected override void Start()
        {
            base.Start();
            IsDisplayingText = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) {
                skipTextQueued = true;
            }
        }

        public void DisplayDialoguePrompt(DialoguePromptData dialogue, bool overwriteExistingDialogue = false)
        {
            faceImage.sprite = dialogue.Face;
            bool canDisplay = !isDisplayingText || overwriteExistingDialogue;
            if (canDisplay)
            {
                StopCoroutine(nameof(TypeDialogue));
                StartCoroutine(nameof(TypeDialogue), dialogue);
            }
        }

        private IEnumerator TypeDialogue(DialoguePromptData dialogue)
        {

            IsDisplayingText = true;

            string[] lines = dialogue.DialogueLines;
            for (int i = 0; i < lines.Length; i++)
            {

                // Display line
                string textLineSoFar = "";
                string line = lines[i];

                for (int j = 0; j < line.Length; j++)
                {

                    // Display text
                    textLineSoFar += line[j];
                    dialogueText.SetText(textLineSoFar);

                    if (skipTextQueued)
                    {
                        skipTextQueued = false;
                        break;
                    }

                    yield return new WaitForSeconds(characterTypeTime);
                }
                dialogueText.SetText(line);

                bool lastLine = i == (lines.Length - 1);
                if (lastLine) {
                    
                }

                while (!skipTextQueued)
                {
                    yield return null;
                }

                skipTextQueued = false;


                dialogueText.SetText("");
            }

            IsDisplayingText = false;
            LevelManager.Instance.EndDialogue();
        }
    }
}
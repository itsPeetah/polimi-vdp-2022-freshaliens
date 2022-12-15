using UnityEngine;

[CreateAssetMenu(menuName = "Freshaliens/Dialogue Prompt", fileName = "New Dialogue Prompt")]
public class DialoguePromptData : ScriptableObject
{
    [SerializeField] private Sprite faceSprite;
    [SerializeField, TextArea] private string[] dialogueText;

    public Sprite Face => faceSprite;
    public string[] DialogueLines => dialogueText;
}

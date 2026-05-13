using UnityEngine;
using UnityEngine.UI;

public class DialogueEditorUI : MonoBehaviour
{
    [SerializeField]
    private InputField inputField; 
    
    [SerializeField]
    private Dropdown typeDropdown;

    private CharacterDialogueRuntime
        currentCharacter;

    public void SetCharacter(
        CharacterDialogueRuntime target)
    {
        currentCharacter = target;
    }

    public void AddDialogue()
    {
        if (currentCharacter == null)
            return;

        string text =
            inputField.text;

        if (string.IsNullOrEmpty(text))
            return;

        DialogueType type =
            (DialogueType)
            typeDropdown.value;

        currentCharacter
            .AddDialogue(
                type,
                text);

        inputField.text = string.Empty;
    }
}
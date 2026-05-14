using UnityEngine;
using UnityEngine.UI;

public class DialogueEditorUI : MonoBehaviour
{
    [SerializeField]
    private InputField scriptInputField;

    [SerializeField]
    private Dropdown typeDropdown;

    private CharacterDialogueRuntime
        currentCharacter;

    [SerializeField]
    private CharacterDialogueRuntime
    testCharacter;

    private void Start()
    {
        SetCharacter(testCharacter);
    }

    public void SetCharacter(
    CharacterDialogueRuntime target)
    {
        currentCharacter = target;

        scriptInputField.text =
            target.GetScript();
    }

    public void AddDialogue()
    {
        if (currentCharacter == null)
            return;

        string text =
            scriptInputField.text;

        if (string.IsNullOrEmpty(text))
            return;

        DialogueType type =
            (DialogueType)
            typeDropdown.value;

        currentCharacter.SetScript(text);

        scriptInputField.text = string.Empty;
    }

    public void ApplyScript()
    {
        if (currentCharacter == null)
            return;

        currentCharacter.SetScript(
            scriptInputField.text);
    }
}
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public DialogueType type;

    [TextArea]
    public string[] lines;
}
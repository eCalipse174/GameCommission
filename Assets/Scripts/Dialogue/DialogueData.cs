using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public DialogueType type;
    public GrowthStage stage;

    [TextArea]
    public string[] lines;
}
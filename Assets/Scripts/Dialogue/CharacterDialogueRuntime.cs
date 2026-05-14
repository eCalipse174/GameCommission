using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogueRuntime
    : MonoBehaviour
{
    [SerializeField]
    [TextArea(20, 40)]
    private string dialogueScript;

    private Dictionary
    <DialogueType,
    List<DialogueEntry>>
    dialogueMap;

    private CharacterGrowth growth;

    private void Awake()
    {
        growth =
            GetComponent<CharacterGrowth>();

        BuildRuntimeData();
    }

    private void BuildRuntimeData()
    {
        dialogueMap =
            DialogueParser.Parse(
                dialogueScript);
    }

    public string GetRandomDialogue(
        DialogueType type)
    {
        if (!dialogueMap.ContainsKey(type))
        {
            return string.Empty;
        }

        List<DialogueEntry> valid =
            new List<DialogueEntry>();

        foreach (DialogueEntry entry
            in dialogueMap[type])
        {
            if (entry.stage ==
                growth.CurrentStage)
            {
                valid.Add(entry);
            }
        }

        if (valid.Count == 0)
        {
            return string.Empty;
        }

        int index =
            Random.Range(
                0,
                valid.Count);

        return valid[index].text;
    }

    public void SetScript(
        string script)
    {
        dialogueScript = script;

        BuildRuntimeData();
    }

    public string GetScript()
    {
        return dialogueScript;
    }
}
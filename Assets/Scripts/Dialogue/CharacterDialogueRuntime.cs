using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogueRuntime : MonoBehaviour
{
    private Dictionary<DialogueType, List<DialogueEntry>> dialogueMap;
    private CharacterGrowth growth;
    private CharacterController controller;

    private void Awake()
    {
        growth = GetComponent<CharacterGrowth>();
        controller = GetComponent<CharacterController>();
        BuildRuntimeData();
    }

    private void BuildRuntimeData()
    {
        dialogueMap = new Dictionary<DialogueType, List<DialogueEntry>>();

        DialogueData[] dialogues = controller.Data.dialogues;
        if (dialogues == null) return;

        foreach (DialogueData data in dialogues)
        {
            if (!dialogueMap.ContainsKey(data.type))
            {
                dialogueMap.Add(data.type, new List<DialogueEntry>());
            }

            foreach (string line in data.lines)
            {
                dialogueMap[data.type].Add(
                    new DialogueEntry { text = line, stage = data.stage });
            }
        }
    }

    public string GetRandomDialogue(DialogueType type)
    {
        if (!dialogueMap.ContainsKey(type))
        {
            return string.Empty;
        }

        List<DialogueEntry> valid = new List<DialogueEntry>();
        foreach (DialogueEntry entry in dialogueMap[type])
        {
            if (entry.stage == growth.CurrentStage)
            {
                valid.Add(entry);
            }
        }

        if (valid.Count == 0)
        {
            return string.Empty;
        }

        int index = Random.Range(0, valid.Count);
        return valid[index].text;
    }
}
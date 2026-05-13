using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogueRuntime : MonoBehaviour
{
    private Dictionary<DialogueType,
        List<string>> dialogueMap =
        new Dictionary<DialogueType,
        List<string>>();

    private CharacterController controller;

    private void Awake()
    {
        controller =
            GetComponent<CharacterController>();

        BuildRuntimeData();
    }

    private void BuildRuntimeData()
    {
        foreach (DialogueData data
            in controller.Data.dialogues)
        {
            if (!dialogueMap.ContainsKey(
                data.type))
            {
                dialogueMap.Add(
                    data.type,
                    new List<string>());
            }

            dialogueMap[data.type]
                .AddRange(data.lines);
        }
    }

    public string GetRandomDialogue(
        DialogueType type)
    {
        if (!dialogueMap.ContainsKey(type))
            return string.Empty;

        List<string> list =
            dialogueMap[type];

        if (list.Count == 0)
            return string.Empty;

        return list[
            Random.Range(0, list.Count)];
    }

    public void AddDialogue(
        DialogueType type,
        string line)
    {
        if (!dialogueMap.ContainsKey(type))
        {
            dialogueMap.Add(
                type,
                new List<string>());
        }

        dialogueMap[type].Add(line);
    }

    public void RemoveDialogue(
        DialogueType type,
        string line)
    {
        if (!dialogueMap.ContainsKey(type))
            return;

        dialogueMap[type]
            .Remove(line);
    }

    public List<string> GetDialogues(
        DialogueType type)
    {
        if (!dialogueMap.ContainsKey(type))
        {
            return new List<string>();
        }

        return dialogueMap[type];
    }
}
using System;
using System.Collections.Generic;

public static class DialogueParser
{
    public static Dictionary
    <DialogueType, List<DialogueEntry>>
    Parse(string script)
    {
        Dictionary
        <DialogueType,
        List<DialogueEntry>>
        result =
            new Dictionary
            <DialogueType,
            List<DialogueEntry>>();

        DialogueType currentType =
            DialogueType.Idle;

        GrowthStage currentStage =
            GrowthStage.Baby;

        string[] lines =
            script.Split('\n');

        foreach (string raw in lines)
        {
            string line =
                raw.Trim();

            if (string.IsNullOrEmpty(line))
                continue;

            if (line.StartsWith("[")
                && line.EndsWith("]"))
            {
                string content =
                    line.Substring(
                        1,
                        line.Length - 2);

                string[] split =
                    content.Split(':');

                if (split.Length >= 1)
                {
                    Enum.TryParse(
                        split[0],
                        out currentType);
                }

                if (split.Length >= 2)
                {
                    Enum.TryParse(
                        split[1],
                        out currentStage);
                }

                if (!result.ContainsKey(
                    currentType))
                {
                    result.Add(
                        currentType,
                        new List<DialogueEntry>());
                }

                continue;
            }

            if (!result.ContainsKey(
                currentType))
            {
                result.Add(
                    currentType,
                    new List<DialogueEntry>());
            }

            result[currentType]
                .Add(
                    new DialogueEntry
                    {
                        text = line,
                        stage = currentStage
                    });
        }

        return result;
    }
}
using System;
using System.Collections.Generic;

public static class DialogueParser
{
    public static Dictionary
    <DialogueType, List<string>>
    Parse(string script)
    {
        Dictionary
        <DialogueType, List<string>>
        result =
            new Dictionary
            <DialogueType, List<string>>();

        DialogueType currentType =
            DialogueType.Idle;

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
                string typeName =
                    line.Substring(
                        1,
                        line.Length - 2);

                if (Enum.TryParse(
                    typeName,
                    out DialogueType type))
                {
                    currentType = type;

                    if (!result.ContainsKey(type))
                    {
                        result.Add(
                            type,
                            new List<string>());
                    }
                }

                continue;
            }

            if (!result.ContainsKey(
                currentType))
            {
                result.Add(
                    currentType,
                    new List<string>());
            }

            result[currentType]
                .Add(line);
        }

        return result;
    }
}
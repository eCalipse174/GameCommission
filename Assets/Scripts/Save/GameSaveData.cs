using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public List<CharacterSaveData>
        characters =
            new List<CharacterSaveData>();
}
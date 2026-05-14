using System;

[Serializable]
public class CharacterSaveData
{
    public string characterId;

    public float posX;
    public float posY;

    public int growthPoint;

    public GrowthStage growthStage;

    public string dialogueScript;
}
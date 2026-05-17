using UnityEngine;

[CreateAssetMenu(menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Profile")]

    public string characterName;

    public GenderType gender;

    [TextArea(3, 6)]
    public string description;

    [Space]

    [Header("Movement")]
    public float moveSpeed;

    [Header("Random Move")]
    public float moveRadius;

    public float minIdleTime;
    public float maxIdleTime;

    [Header("Sprite")]
    public Sprite idleSprite;

    [Header("Needs")]
    public NeedData[] needs;

    [Header("Favorite")]
    public ItemType favoriteFood;
    public ItemType favoriteToy;

    [Header("Need Requirements")]
    public NeedRequirement[] needRequirements;

    [Header("Emotion Sprites")]
    public Sprite normalSprite;
    public Sprite happySprite;
    public Sprite sadSprite;
    public Sprite angrySprite;
    public Sprite sleepySprite;
    public Sprite surprisedSprite;

    [Header("Dialogue")]
    public DialogueData[] dialogues;
}
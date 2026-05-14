using UnityEngine;

public class CharacterSaveIdentity
    : MonoBehaviour
{
    [SerializeField]
    private string characterId;

    public string CharacterId =>
        characterId;
}
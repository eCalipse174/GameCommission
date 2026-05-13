using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField]
    private ItemType itemType;

    public ItemType ItemType => itemType;
}
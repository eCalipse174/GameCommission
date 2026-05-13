using UnityEngine;

public class CharacterNeedResolver : MonoBehaviour
{
    private CharacterController controller;

    private NeedSystem needSystem;

    private void Awake()
    {
        controller =
            GetComponent<CharacterController>();

        needSystem =
            GetComponent<NeedSystem>();
    }

    public bool TryResolve(ItemObject item)
    {
        NeedType currentNeed =
            needSystem.CurrentNeed;

        if (currentNeed == NeedType.None)
        {
            Reject(item);

            return false;
        }

        switch (currentNeed)
        {
            case NeedType.Hunger:
                return HandleFood(item);
        }

        Reject(item);

        return false;
    }

    private bool HandleFood(ItemObject item)
    {
        if (IsFavoriteFood(item.ItemType))
        {
            Resolve(item);

            return true;
        }

        Reject(item);

        return false;
    }

    private bool IsFavoriteFood(ItemType itemType)
    {
        return itemType.ToString() ==
               controller.Data.favoriteFood.ToString();
    }

    private void Resolve(ItemObject item)
    {
        Debug.Log(
            $"{name} accepted {item.ItemType}");

        needSystem.ResolveNeed();
    }

    private void Reject(ItemObject item)
    {
        Debug.Log(
            $"{name} rejected {item.ItemType}");

        controller.EmotionSystem
            .SetEmotion(
                EmotionType.Angry);

        DragItem dragItem =
            item.GetComponent<DragItem>();

        dragItem.ReturnToStartPosition();
    }
}
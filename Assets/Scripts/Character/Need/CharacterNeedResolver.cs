using UnityEngine;

public class CharacterNeedResolver
    : MonoBehaviour
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

    public bool TryResolve(
        ItemObject item)
    {
        NeedType currentNeed =
            needSystem.CurrentNeed;

        if (currentNeed ==
            NeedType.None)
        {
            Ignore(item);

            return false;
        }

        switch (currentNeed)
        {
            case NeedType.Hunger:
                return HandleFood(item);

            case NeedType.Play:
                return HandleToy(item);

            case NeedType.Sleep:
                return HandleBed(item);

            case NeedType.Dirty:
                return HandleClean(item);
        }

        Ignore(item);

        return false;
    }

    private bool HandleFood(
        ItemObject item)
    {
        if (IsFavoriteFood(
            item.ItemType))
        {
            Resolve(item);

            return true;
        }

        Reject(item);

        return false;
    }

    private bool HandleToy(
        ItemObject item)
    {
        if (item.ItemType ==
            ItemType.Toy)
        {
            Resolve(item);

            return true;
        }

        Reject(item);

        return false;
    }

    private bool HandleBed(
        ItemObject item)
    {
        if (item.ItemType ==
            ItemType.Bed)
        {
            Resolve(item);

            return true;
        }

        Reject(item);

        return false;
    }

    private bool HandleClean(
        ItemObject item)
    {
        if (item.ItemType ==
            ItemType.Soap ||
            item.ItemType ==
            ItemType.Towel)
        {
            Resolve(item);

            return true;
        }

        Reject(item);

        return false;
    }

    private bool IsFavoriteFood(
        ItemType itemType)
    {
        return itemType ==
               controller.Data
                   .favoriteFood;
    }

    private void Resolve(
        ItemObject item)
    {
        Debug.Log(
            $"{name} accepted {item.ItemType}");

        needSystem.ResolveNeed();

        controller.EmotionSystem
            .SetEmotion(
                EmotionType.Happy);
    }

    private void Reject(
        ItemObject item)
    {
        Debug.Log(
            $"{name} rejected {item.ItemType}");

        controller.EmotionSystem
            .SetEmotion(
                EmotionType.Angry);

        DragItem dragItem =
            item.GetComponent<DragItem>();

        if (dragItem != null)
        {
            dragItem.ReturnToStartPosition();
        }
    }

    private void Ignore(
        ItemObject item)
    {
        DragItem dragItem =
            item.GetComponent<DragItem>();

        if (dragItem != null)
        {
            dragItem.ReturnToStartPosition();
        }
    }
}
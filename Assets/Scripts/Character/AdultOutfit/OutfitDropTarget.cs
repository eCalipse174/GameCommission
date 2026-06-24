using UnityEngine;

public class OutfitDropTarget : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    public bool TryApplyOutfit(AdultOutfit outfit, OutfitManager outfitManager)
    {
        if (!controller.Data) return false;

        // 성체가 아니면 옷 갈아입기 불가
        CharacterGrowth growth = controller.GetComponent<CharacterGrowth>();
        if (growth == null || !growth.IsAdult)
        {
            Debug.Log($"{name} is not adult yet, cannot change outfit");
            return false;
        }

        bool success = outfitManager.TrySetOutfit(outfit.outfitId);

        if (success)
        {
            controller.EmotionSystem.SetEmotion(EmotionType.Happy);
        }

        return success;
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class ClosetOutfitSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private OutfitDragItem dragItemPrefab;
    [SerializeField] private Camera worldCamera;

    private AdultOutfit outfit;
    private OutfitManager targetOutfitManager;

    public void Setup(AdultOutfit outfit, OutfitManager targetOutfitManager)
    {
        this.outfit = outfit;
        this.targetOutfitManager = targetOutfitManager;

        if (outfit.thumbnail != null)
            iconImage.sprite = outfit.thumbnail;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Camera cam = worldCamera != null ? worldCamera : Camera.main;

        Vector3 screenPos = new Vector3(
            eventData.position.x,
            eventData.position.y,
            10f);

        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);

        OutfitDragItem dragItem = Instantiate(
            dragItemPrefab,
            worldPos,
            Quaternion.identity);

        dragItem.BeginDrag(outfit, targetOutfitManager);
    }
}
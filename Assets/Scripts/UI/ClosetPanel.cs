using UnityEngine;
using UnityEngine.UI;

public class ClosetPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private ClosetOutfitSlot slotPrefab;

    public bool IsOpen => panelRoot.activeSelf;

    public void Open(OutfitManager targetOutfitManager)
    {
        ClearSlots();

        foreach (AdultOutfit outfit in targetOutfitManager.AvailableOutfits)
        {
            ClosetOutfitSlot slot = Instantiate(slotPrefab, slotContainer);
            slot.Setup(outfit, targetOutfitManager);
        }

        panelRoot.SetActive(true);
    }

    public void Close()
    {
        panelRoot.SetActive(false);
    }

    private void ClearSlots()
    {
        for (int i = slotContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(slotContainer.GetChild(i).gameObject);
        }
    }
}
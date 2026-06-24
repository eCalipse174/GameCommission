using UnityEngine;

public class ClosetButton : MonoBehaviour
{
    [SerializeField] private ClosetPanel closetPanel;
    [SerializeField] private OutfitManager targetOutfitManager;

    public void OnClickOpenCloset()
    {
        closetPanel.Open(targetOutfitManager);
    }
}
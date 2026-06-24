using UnityEngine;
using UnityEngine.UI;

public class ClosetButton : MonoBehaviour
{
    [SerializeField] private ClosetPanel closetPanel;
    [SerializeField] private OutfitManager targetOutfitManager;
    [SerializeField] private CharacterGrowth targetGrowth;
    [SerializeField] private Button button;

    private void OnEnable()
    {
        if (targetGrowth != null)
            targetGrowth.OnStageChanged += HandleStageChanged;
    }

    private void OnDisable()
    {
        if (targetGrowth != null)
            targetGrowth.OnStageChanged -= HandleStageChanged;
    }

    private void HandleStageChanged(GrowthStage stage)
    {
        button.interactable = stage == GrowthStage.Adult;
    }

    public void OnClickToggleCloset()
    {
        if (targetGrowth == null || !targetGrowth.IsAdult)
            return;

        if (closetPanel.IsOpen)
            closetPanel.Close();
        else
            closetPanel.Open(targetOutfitManager);
    }
}
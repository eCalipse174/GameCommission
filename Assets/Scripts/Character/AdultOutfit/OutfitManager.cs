using System;
using UnityEngine;

public class OutfitManager : MonoBehaviour
{
    [SerializeField] private AdultOutfit[] availableOutfits;
    [SerializeField] private CharacterAnimator characterAnimator;

    private AdultOutfit currentOutfit;

    public AdultOutfit CurrentOutfit => currentOutfit;
    public AdultOutfit[] AvailableOutfits => availableOutfits;

    public event Action<AdultOutfit> OnOutfitChanged;

    private void Awake()
    {
        if (availableOutfits.Length > 0)
            currentOutfit = availableOutfits[0];
    }

    public bool TrySetOutfit(string outfitId)
    {
        AdultOutfit outfit = FindOutfit(outfitId);
        if (outfit == null)
        {
            Debug.LogWarning($"Outfit not found: {outfitId}");
            return false;
        }

        currentOutfit = outfit;
        characterAnimator.SetOutfit(currentOutfit);
        OnOutfitChanged?.Invoke(currentOutfit);
        return true;
    }

    public void ApplyCurrentOutfit()
    {
        if (currentOutfit != null)
            characterAnimator.SetOutfit(currentOutfit);
    }

    private AdultOutfit FindOutfit(string outfitId)
    {
        foreach (var outfit in availableOutfits)
        {
            if (outfit.outfitId == outfitId)
                return outfit;
        }
        return null;
    }
}
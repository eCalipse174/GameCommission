using UnityEngine;

[CreateAssetMenu(menuName = "Game/Adult Outfit")]
public class AdultOutfit : ScriptableObject
{
    [Header("Identity")]
    public string outfitId;
    public string displayName;
    public Sprite thumbnail; // UI에 표시할 미리보기 아이콘

    [Header("Emotion Sets")]
    public EmotionAnimationSet normalSet;
    public EmotionAnimationSet happySet;
    public EmotionAnimationSet sadSet;
    public EmotionAnimationSet angrySet;
    public EmotionAnimationSet surprisedSet;

    public EmotionAnimationSet GetSet(EmotionType emotion)
    {
        switch (emotion)
        {
            case EmotionType.Happy: return happySet;
            case EmotionType.Sad: return sadSet;
            case EmotionType.Angry: return angrySet;
            case EmotionType.Surprised: return surprisedSet;
        }
        return normalSet;
    }
}
using UnityEngine;

public class CharacterEmotionDisplay
    : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer faceRenderer;

    [SerializeField]
    private EmotionSpriteSet spriteSet;

    public void SetEmotion(
        EmotionType emotion)
    {
        faceRenderer.sprite =
            spriteSet.GetSprite(
                emotion);
    }
}
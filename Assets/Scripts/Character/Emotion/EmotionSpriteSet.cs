using UnityEngine;

[CreateAssetMenu(
    menuName =
    "Game/EmotionSpriteSet")]
public class EmotionSpriteSet
    : ScriptableObject
{
    public Sprite idle;

    public Sprite happy;

    public Sprite sad;

    public Sprite angry;

    public Sprite surprised;

    public Sprite sleepy;

    public Sprite GetSprite(
        EmotionType emotion)
    {
        switch (emotion)
        {
            case EmotionType.Happy:
                return happy;

            case EmotionType.Sad:
                return sad;

            case EmotionType.Angry:
                return angry;

            case EmotionType.Surprised:
                return surprised;

            case EmotionType.Sleepy:
                return sleepy;
        }

        return idle;
    }
}
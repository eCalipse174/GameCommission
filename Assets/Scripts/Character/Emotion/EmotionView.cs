using UnityEngine;

public class EmotionView : MonoBehaviour
{
    private CharacterController controller;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        controller =
            GetComponent<CharacterController>();

        spriteRenderer =
            GetComponent<SpriteRenderer>();
    }

    public void UpdateEmotion(
        EmotionType emotion)
    {
        switch (emotion)
        {
            case EmotionType.Normal:
                spriteRenderer.sprite =
                    controller.Data.normalSprite;
                break;

            case EmotionType.Happy:
                spriteRenderer.sprite =
                    controller.Data.happySprite;
                break;

            case EmotionType.Sad:
                spriteRenderer.sprite =
                    controller.Data.sadSprite;
                break;

            case EmotionType.Angry:
                spriteRenderer.sprite =
                    controller.Data.angrySprite;
                break;

            case EmotionType.Sleepy:
                spriteRenderer.sprite =
                    controller.Data.sleepySprite;
                break;

            case EmotionType.Surprised:
                spriteRenderer.sprite =
                    controller.Data.surprisedSprite;
                break;
        }
    }
}
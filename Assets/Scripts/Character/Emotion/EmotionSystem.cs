using UnityEngine;

public class EmotionSystem : MonoBehaviour
{
    private CharacterAnimator animator;

    private EmotionType currentEmotion =
        EmotionType.Normal;

    public EmotionType CurrentEmotion =>
        currentEmotion;

    private void Awake()
    {
        animator = 
            GetComponent<CharacterAnimator>();
    }

    private void Start()
    {
        SetEmotion(EmotionType.Normal);
    }

    public void SetEmotion(
        EmotionType emotion)
    {
        currentEmotion = emotion;
        animator.SetEmotion(emotion);
        Debug.Log(animator.gameObject.name);

        Debug.Log(
            $"{name} Emotion: {emotion}");
    }
}
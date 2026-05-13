using UnityEngine;

public class EmotionSystem : MonoBehaviour
{
    private CharacterController controller;

    private EmotionView emotionView;

    private EmotionType currentEmotion =
        EmotionType.Normal;

    public EmotionType CurrentEmotion =>
        currentEmotion;

    private void Awake()
    {
        controller =
            GetComponent<CharacterController>();

        emotionView =
            GetComponent<EmotionView>();
    }

    private void Start()
    {
        SetEmotion(EmotionType.Normal);
    }

    public void SetEmotion(
        EmotionType emotion)
    {
        currentEmotion = emotion;

        emotionView.UpdateEmotion(
            currentEmotion);

        Debug.Log(
            $"{name} Emotion: {emotion}");
    }
}
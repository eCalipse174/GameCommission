using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    private float minTalkInterval = 8f;

    [SerializeField]
    private float maxTalkInterval = 15f;

    private CharacterController controller;

    private SpeechBubbleView speechBubbleView;

    private CharacterDialogueRuntime runtimeDialogue;

    private float talkTimer;

    private void Awake()
    {
        controller =
            GetComponent<CharacterController>();

        speechBubbleView =
            GetComponentInChildren
            <SpeechBubbleView>();

        runtimeDialogue = GetComponent<CharacterDialogueRuntime>();
    }

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (controller.IsLocked)
        {
            return;
        }

        if (speechBubbleView.IsShowing)
        {
            return;
        }

        talkTimer -= Time.deltaTime;

        if (talkTimer <= 0f)
        {
            TryTalk();

            ResetTimer();
        }
    }

    private void TryTalk()
    {
        if (controller.IsLocked)
            return;

        DialogueType type =
            GetCurrentDialogueType();

        string line =
            GetRandomDialogue(type);

        if (string.IsNullOrEmpty(line))
            return;

        speechBubbleView.Show(line);

        Debug.Log(
            $"{name} says: {line}");
    }

    private DialogueType
        GetCurrentDialogueType()
    {
        switch (controller
            .EmotionSystem
            .CurrentEmotion)
        {
            case EmotionType.Happy:
                return DialogueType.Happy;

            case EmotionType.Angry:
                return DialogueType.Angry;

            case EmotionType.Sleepy:
                return DialogueType.Sleepy;
        }

        if (controller
            .NeedSystem
            .CurrentNeed ==
            NeedType.Hunger)
        {
            return DialogueType.Hunger;
        }

        return DialogueType.Idle;
    }

    private string GetRandomDialogue(
    DialogueType type)
    {
        return runtimeDialogue
            .GetRandomDialogue(type);
    }

    private void ResetTimer()
    {
        talkTimer =
            Random.Range(
                minTalkInterval,
                maxTalkInterval);
    }
}
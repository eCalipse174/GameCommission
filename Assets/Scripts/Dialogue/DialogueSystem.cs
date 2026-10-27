using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    private float minTalkInterval = 8f;

    [SerializeField]
    private float maxTalkInterval = 15f;

    [Header("Sleep Talk")]
    [SerializeField]
    private DialogueType sleepTalkType =
        DialogueType.Sleepy;

    [SerializeField]
    private float minSleepTalkInterval = 10f;

    [SerializeField]
    private float maxSleepTalkInterval = 20f;

    private CharacterController controller;

    private SpeechBubbleView speechBubbleView;

    private CharacterDialogueRuntime runtimeDialogue;

    private SleepController sleepController;

    private float talkTimer;

    private bool IsSleeping =>
        sleepController != null &&
        sleepController.IsSleeping;

    private void Awake()
    {
        controller =
            GetComponent<CharacterController>();

        speechBubbleView =
            GetComponentInChildren
            <SpeechBubbleView>();

        runtimeDialogue = GetComponent<CharacterDialogueRuntime>();

        sleepController =
            GetComponent<SleepController>();
    }

    private void OnEnable()
    {
        if (sleepController != null)
        {
            sleepController.OnSleepStateChanged +=
                HandleSleepStateChanged;
        }
    }

    private void OnDisable()
    {
        if (sleepController != null)
        {
            sleepController.OnSleepStateChanged -=
                HandleSleepStateChanged;
        }
    }

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        // A sleeping character is locked by SleepController,
        // so the lock check must let sleep talk through.
        if (controller.IsLocked && !IsSleeping)
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

    private void HandleSleepStateChanged(bool sleeping)
    {
        ResetTimer();
    }

    private void TryTalk()
    {
        bool sleeping = IsSleeping;

        if (controller.IsLocked && !sleeping)
            return;

        DialogueType type =
            sleeping
            ? sleepTalkType
            : GetCurrentDialogueType();

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
        float min =
            IsSleeping
            ? minSleepTalkInterval
            : minTalkInterval;

        float max =
            IsSleeping
            ? maxSleepTalkInterval
            : maxTalkInterval;

        talkTimer =
            Random.Range(min, max);
    }
}
using UnityEngine;

public class NeedSystem : MonoBehaviour
{
    [SerializeField]
    private NeedView needView;

    private CharacterController controller;

    private NeedType currentNeed =
        NeedType.None;

    private float needTimer;

    private NeedData currentNeedData;

    public NeedType CurrentNeed => currentNeed;

    private void Awake()
    {
        controller =
            GetComponent<CharacterController>();
    }

    private void Start()
    {
        ResetNeedTimer();
    }

    private void Update()
    {
        if (currentNeed != NeedType.None)
            return;

        needTimer -= Time.deltaTime;

        if (needTimer <= 0f)
        {
            GenerateRandomNeed();
        }
    }

    public bool HasNeed()
    {
        return currentNeed != NeedType.None;
    }

    public void ResolveNeed()
    {
        currentNeed = NeedType.None;

        needView.Hide();

        controller.EmotionSystem
            .SetEmotion(
                EmotionType.Happy);

        Invoke(nameof(ReturnToNormal), 2f);

        ResetNeedTimer();
    }

    private void ReturnToNormal()
    {
        controller.EmotionSystem
            .SetEmotion(
                EmotionType.Normal);
    }

    private void GenerateRandomNeed()
    {
        NeedData[] needs =
            controller.Data.needs;

        if (needs.Length == 0)
            return;

        int randomIndex =
            Random.Range(0, needs.Length);

        currentNeedData =
            needs[randomIndex];

        currentNeed =
            currentNeedData.type;

        needView.Show(currentNeed);

        switch (currentNeed)
        {
            case NeedType.Hunger:
                controller.EmotionSystem
                    .SetEmotion(
                        EmotionType.Sad);
                break;

            case NeedType.Sleep:
                controller.EmotionSystem
                    .SetEmotion(
                        EmotionType.Sleepy);
                break;

            case NeedType.Play:
                controller.EmotionSystem
                    .SetEmotion(
                        EmotionType.Sad);
                break;
        }

        Debug.Log(
            $"{name} NEED: {currentNeed}");
    }

    private void ResetNeedTimer()
    {
        NeedData[] needs =
            controller.Data.needs;

        if (needs.Length == 0)
            return;

        int randomIndex =
            Random.Range(0, needs.Length);

        NeedData randomNeed =
            needs[randomIndex];

        needTimer =
            Random.Range(
                randomNeed.minInterval,
                randomNeed.maxInterval);
    }
}
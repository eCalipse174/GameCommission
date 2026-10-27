using UnityEngine;

public class CharacterDialogueAgent
    : MonoBehaviour
{
    [SerializeField]
    private float interactionRange = 2f;

    [SerializeField]
    private float interactionCooldown = 8f;

    private float cooldown;

    private CharacterController controller;

    private SleepController sleepController;

    public bool Busy =>
        controller.IsLocked ||
        IsSleeping;

    private bool IsSleeping =>
        sleepController != null &&
        sleepController.IsSleeping;

    private void Awake()
    {
        controller =
            GetComponent<CharacterController>();

        sleepController =
            GetComponent<SleepController>();
    }

    private void Update()
    {
        if (Busy)
            return;

        cooldown -= Time.deltaTime;

        if (cooldown > 0f)
            return;

        TryStartInteraction();
    }

    private void TryStartInteraction()
    {
        CharacterDialogueAgent[]
            agents =
            FindObjectsByType
            <CharacterDialogueAgent>(
                FindObjectsSortMode.None);

        foreach (var other in agents)
        {
            if (other == this)
                continue;

            if (other.Busy)
                continue;

            float distance =
                Vector2.Distance(
                    transform.position,
                    other.transform.position);

            if (distance > interactionRange)
                continue;

            StartInteraction(other);

            break;
        }
    }

    private void StartInteraction(
        CharacterDialogueAgent other)
    {
        cooldown = interactionCooldown;

        other.cooldown =
            interactionCooldown;

        DialogueSession session =
            new DialogueSession(
                this,
                other);

        session.Start();
    }

    public CharacterController
        Controller => controller;
}
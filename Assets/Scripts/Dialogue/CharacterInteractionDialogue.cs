using System.Collections;
using UnityEngine;

public class CharacterInteractionDialogue
    : MonoBehaviour
{
    [SerializeField]
    private float interactionRange = 2f;

    [SerializeField]
    private float interactionInterval = 10f;

    private float timer;

    private CharacterController controller;

    private SpeechBubbleView bubbleView;

    private CharacterDialogueRuntime
        runtimeDialogue;

    private bool interactionRunning;

    private void Awake()
    {
        controller =
            GetComponent<CharacterController>();

        bubbleView =
            GetComponentInChildren
            <SpeechBubbleView>();

        runtimeDialogue =
            GetComponent<
                CharacterDialogueRuntime>();
    }

    private void Update()
    {
        if (interactionRunning)
            return;

        if (controller.IsInteracting)
            return;

        timer -= Time.deltaTime;

        if (timer > 0f)
            return;

        TryInteraction();

        timer = interactionInterval;
    }

    private void TryInteraction()
    {
        CharacterInteractionDialogue[]
    others =
    FindObjectsByType
    <CharacterInteractionDialogue>(
        FindObjectsSortMode.None);

        foreach (var other in others)
        {
            if (other == this)
                continue;

            if (other.controller
                .IsInteracting)
            {
                continue;
            }

            float distance =
                Vector2.Distance(
                    transform.position,
                    other.transform.position);

            if (distance > interactionRange)
                continue;

            StartCoroutine(
                InteractionRoutine(other));

            break;
        }
    }

    private IEnumerator InteractionRoutine(
        CharacterInteractionDialogue other)
    {
        interactionRunning = true;

        controller.StartInteraction();

        other.controller
            .StartInteraction();

        string myLine =
            runtimeDialogue
            .GetRandomDialogue(
                DialogueType.Happy);

        if (!string.IsNullOrEmpty(
            myLine))
        {
            bubbleView.Show(myLine);
        }

        yield return new WaitUntil(
            () => !bubbleView.IsShowing);

        yield return new WaitForSeconds(
            0.5f);

        string otherLine =
            other.runtimeDialogue
            .GetRandomDialogue(
                DialogueType.Happy);

        if (!string.IsNullOrEmpty(
            otherLine))
        {
            other.bubbleView
                .Show(otherLine);
        }

        yield return new WaitUntil(
            () => !other.bubbleView
                .IsShowing);

        controller.EndInteraction();

        other.controller
            .EndInteraction();

        interactionRunning = false;
    }
}
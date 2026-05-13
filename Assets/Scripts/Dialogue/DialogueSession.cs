using System.Collections;
using UnityEngine;

public class DialogueSession
{
    private CharacterDialogueAgent a;

    private CharacterDialogueAgent b;

    public DialogueSession(
        CharacterDialogueAgent a,
        CharacterDialogueAgent b)
    {
        this.a = a;
        this.b = b;
    }

    public void Start()
    {
        a.StartCoroutine(
            RunSession());
    }

    private IEnumerator RunSession()
    {
        CharacterController aController =
            a.Controller;

        CharacterController bController =
            b.Controller;

        aController.Lock();
        bController.Lock();

        FaceEachOther(
            aController,
            bController);

        yield return new WaitForSeconds(
            0.5f);

        yield return TalkTurn(
            aController,
            DialogueType.InteractionStart);

        yield return new WaitForSeconds(
            0.5f);

        yield return TalkTurn(
            bController,
            DialogueType.InteractionReply);

        yield return new WaitForSeconds(
            0.5f);

        yield return TalkTurn(
            aController,
            DialogueType.InteractionReply);

        yield return new WaitForSeconds(
            1f);

        aController.Unlock();
        bController.Unlock();
    }

    private IEnumerator TalkTurn(
        CharacterController controller,
        DialogueType type)
    {
        CharacterDialogueRuntime runtime =
            controller.GetComponent
            <CharacterDialogueRuntime>();

        SpeechBubbleView bubble =
            controller.GetComponentInChildren
            <SpeechBubbleView>();

        string line =
            runtime.GetRandomDialogue(type);

        if (string.IsNullOrEmpty(line))
            yield break;

        bool success = bubble.Show(line);

        if (!success)
        {
            yield break;
        }

        yield return new WaitUntil(
            () => !bubble.IsShowing);
    }

    private void FaceEachOther(
        CharacterController a,
        CharacterController b)
    {
        SpriteRenderer aRenderer =
            a.GetComponent<SpriteRenderer>();

        SpriteRenderer bRenderer =
            b.GetComponent<SpriteRenderer>();

        bool aLeft =
            a.transform.position.x >
            b.transform.position.x;

        aRenderer.flipX = aLeft;

        bRenderer.flipX = !aLeft;
    }
}
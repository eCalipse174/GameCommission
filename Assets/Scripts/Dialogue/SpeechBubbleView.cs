using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class SpeechBubbleView : MonoBehaviour
{
    [SerializeField]
    private GameObject root;

    [SerializeField]
    private Text dialogueText;

    [SerializeField]
    private float visibleDuration = 3f;

    private Coroutine currentRoutine;

    private bool showing;
    public bool IsShowing => showing;

    private void Start()
    {
        root.SetActive(false);
    }

    public bool Show(string message)
    {
        if (showing)
        {
            return false;
        }

        currentRoutine =
            StartCoroutine(
                ShowRoutine(message));

        return true;
    }

    private IEnumerator ShowRoutine(
    string message)
    {
        showing = true;

        root.SetActive(true);

        dialogueText.text = message;

        yield return new WaitForSeconds(
            visibleDuration);

        root.SetActive(false);

        showing = false;
    }
}
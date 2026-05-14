using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelectable
    : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField]
    private float holdTime = 0.5f;

    private bool pressing;

    private float timer;

    private CharacterDialogueRuntime
        runtime;

    private void Awake()
    {
        runtime =
            GetComponent<
                CharacterDialogueRuntime>();
    }

    private void Update()
    {
        if (!pressing)
            return;

        timer += Time.deltaTime;

        if (timer >= holdTime)
        {
            pressing = false;

            CharacterDetailUI.Instance
                .Open(runtime);
        }
    }

    public void OnPointerDown(
        PointerEventData eventData)
    {
        pressing = true;

        timer = 0f;
    }

    public void OnPointerUp(
        PointerEventData eventData)
    {
        pressing = false;
    }
}
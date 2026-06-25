using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    private CharacterController controller;
    private Camera mainCamera;
    private CharacterAnimator characterAnimator;
    private CharacterInputState currentState;
    private bool pointerDown;
    private bool dragging;
    private bool canStartDrag;
    private float pressTimer;
    private Vector3 dragOffset;

    [SerializeField] private CharacterContextMenu contextMenu;

    private const float LONG_PRESS_TIME = 0f;
    private const float DRAG_HOLD_TIME = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        characterAnimator = GetComponent<CharacterAnimator>();
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        pointerDown = true;
        dragging = false;
        canStartDrag = false;
        pressTimer = 0f;
        dragOffset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnRightClick();
        }
    }

    private void OnMouseUp()
    {
        if (!dragging && pressTimer < LONG_PRESS_TIME)
        {
            OnTap();
        }
        pointerDown = false;
        dragging = false;
        canStartDrag = false;
        currentState = CharacterInputState.None;
        characterAnimator.SetDragging(dragging);
        controller.ResumeAI();
    }

    private void Update()
    {
        if (!pointerDown)
            return;

        pressTimer += Time.deltaTime;

        if (pressTimer >= DRAG_HOLD_TIME)
        {
            canStartDrag = true;
        }

        if (!dragging && canStartDrag)
        {
            if (Input.GetMouseButton(0))
            {
                float distance = Vector2.Distance(
                    transform.position, GetMouseWorldPosition());
                if (distance > 0.2f)
                {
                    StartDrag();
                }
            }
        }

        if (dragging)
        {
            Drag();
        }

        if (pressTimer >= LONG_PRESS_TIME &&
            !dragging &&
            currentState != CharacterInputState.LongPress)
        {
            OnLongPress();
        }
    }

    private void OnRightClick()
    {
        controller.PauseAI();
        contextMenu.Open(controller, () => controller.ResumeAI());
    }

    private void StartDrag()
    {
        dragging = true;
        characterAnimator.SetDragging(dragging);
        currentState = CharacterInputState.Dragging;
        controller.PauseAI();
        controller.Movement.Stop();
    }

    private void Drag()
    {
        Vector3 target = GetMouseWorldPosition() + dragOffset;
        Vector2 clamped = MovementArea.Instance.ClampPosition(target);
        transform.position = new Vector3(clamped.x, clamped.y, transform.position.z);
    }

    private void OnTap()
    {
        Debug.Log($"{name} TAP");
        controller.PauseAI();
        Invoke(nameof(ResumeAI), 1f);
    }

    private void OnLongPress()
    {
        currentState = CharacterInputState.LongPress;
        controller.PauseAI();
        Debug.Log($"{name} LONG PRESS");
    }

    private void ResumeAI()
    {
        controller.ResumeAI();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}
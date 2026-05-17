using UnityEngine;

public class CharacterMovement
    : MonoBehaviour
{
    private CharacterController controller;

    private float moveSpeed;

    private Vector2 targetPosition;

    private Vector2 moveDirection;

    private bool isMoving;

    public Vector2 MoveDirection =>
        moveDirection;

    private void Awake()
    {
        controller =
            GetComponent<CharacterController>();
    }

    public void Initialize(
        float speed)
    {
        moveSpeed = speed;
    }

    public void SetTarget(
        Vector2 target)
    {
        targetPosition =
            MovementArea.Instance
                .ClampPosition(
                    target);

        isMoving = true;
    }

    public void SetMoveSpeed(
        float speed)
    {
        moveSpeed = speed;
    }

    public void Stop()
    {
        isMoving = false;

        moveDirection =
            Vector2.zero;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public bool ReachedTarget()
    {
        return Vector2.Distance(
            transform.position,
            targetPosition) < 0.05f;
    }

    private void Update()
    {
        if (controller.IsLocked)
            return;



        if (!isMoving)
            return;

        Vector2 currentPosition =
            transform.position;

        moveDirection =
            (
                targetPosition -
                currentPosition
            ).normalized;

        Vector2 nextPosition =
            Vector2.MoveTowards(
                currentPosition,
                targetPosition,
                moveSpeed *
                Time.deltaTime
            );

        nextPosition =
            MovementArea.Instance
                .ClampPosition(
                    nextPosition);

        transform.position =
            nextPosition;

        if (ReachedTarget())
        {
            isMoving = false;

            moveDirection =
                Vector2.zero;
        }
    }
}
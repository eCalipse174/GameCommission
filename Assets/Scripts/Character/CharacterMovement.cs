using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller;

    private float moveSpeed;

    private Vector2 targetPosition;

    private bool isMoving;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Initialize(float speed)
    {
        moveSpeed = speed;
    }

    public void SetTarget(Vector2 target)
    {
        targetPosition = target;

        isMoving = true;
    }

    public void Stop()
    {
        isMoving = false;
    }

    public bool IsMoving()
    {
        return isMoving;
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

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (ReachedTarget())
        {
            isMoving = false;
        }
    }
}
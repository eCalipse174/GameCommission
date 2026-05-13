using UnityEngine;
using UnityEngine.InputSystem.XR;

public class RandomMoveNode : BTNode
{
    private CharacterController controller;

    private CharacterMovement movement;

    private Transform owner;

    private float moveRadius;

    private bool started;

    public RandomMoveNode(
        CharacterController controller,
        CharacterMovement movement,
        Transform owner,
        float moveRadius)
    {
        this.controller = controller;
        this.movement = movement;
        this.owner = owner;
        this.moveRadius = moveRadius;
    }

    public override NodeState Evaluate()
    {
        if (controller.IsLocked)
        {
            controller.Movement.Stop();

            return NodeState.Failure;
        }

        if (!started)
        {
            Vector2 randomPos =
                (Vector2)owner.position +
                Random.insideUnitCircle * moveRadius;

            movement.SetTarget(randomPos);

            started = true;
        }

        if (movement.IsMoving())
        {
            return NodeState.Running;
        }

        started = false;

        return NodeState.Success;
    }
}
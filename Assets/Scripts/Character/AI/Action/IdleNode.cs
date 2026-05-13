using UnityEngine;

public class IdleNode : BTNode
{
    private CharacterController controller;

    private float idleTime;

    private float timer;

    public IdleNode(float idleTime, CharacterController controller)
    {
        this.controller = controller;
        this.idleTime = idleTime;

        timer = idleTime;
    }

    public override NodeState Evaluate()
    {
        if (controller.IsLocked)
        {
            return NodeState.Failure;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = idleTime;

            return NodeState.Success;
        }

        return NodeState.Running;
    }
}
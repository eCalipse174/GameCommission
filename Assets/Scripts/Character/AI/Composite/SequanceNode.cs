using System.Collections.Generic;

public class SequenceNode : BTNode
{
    private List<BTNode> children;

    private int currentIndex;

    public SequenceNode(List<BTNode> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        while (currentIndex < children.Count)
        {
            NodeState state =
                children[currentIndex].Evaluate();

            if (state == NodeState.Running)
            {
                return NodeState.Running;
            }

            if (state == NodeState.Failure)
            {
                currentIndex = 0;

                return NodeState.Failure;
            }

            currentIndex++;
        }

        currentIndex = 0;

        return NodeState.Success;
    }
}
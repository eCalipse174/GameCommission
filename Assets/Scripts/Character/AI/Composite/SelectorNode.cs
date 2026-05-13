using System.Collections.Generic;

public class SelectorNode : BTNode
{
    private List<BTNode> children;

    public SelectorNode(List<BTNode> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (BTNode node in children)
        {
            NodeState result = node.Evaluate();

            if (result == NodeState.Success ||
                result == NodeState.Running)
            {
                return result;
            }
        }

        return NodeState.Failure;
    }
}
public class NeedNode : BTNode
{
    private NeedSystem needSystem;

    public NeedNode(NeedSystem needSystem)
    {
        this.needSystem = needSystem;
    }

    public override NodeState Evaluate()
    {
        if (!needSystem.HasNeed())
        {
            return NodeState.Failure;
        }

        return NodeState.Running;
    }
}
// Behavior tree node. Place it as the first child of the root selector.
// While the character sleeps it reports Running, so lower priority nodes
// (move, idle, need) are not evaluated. Otherwise it reports Failure and
// the selector falls through to the next node.
public class SleepNode : BTNode
{
    private readonly SleepController controller;

    public SleepNode(SleepController controller)
    {
        this.controller = controller;
    }

    public override NodeState Evaluate()
    {
        return controller.IsSleeping ? NodeState.Running : NodeState.Failure;
    }
}
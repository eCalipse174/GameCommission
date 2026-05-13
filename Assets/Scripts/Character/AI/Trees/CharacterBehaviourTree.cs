public class CharacterBehaviourTree
{
    private BTNode rootNode;

    public CharacterBehaviourTree(BTNode rootNode)
    {
        this.rootNode = rootNode;
    }

    public void Update()
    {
        rootNode.Evaluate();
    }
}
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;

    private CharacterMovement movement;

    private CharacterBehaviourTree behaviourTree;

    private CharacterInteraction interaction;

    private NeedSystem needSystem;

    private EmotionSystem emotionSystem;

    private bool aiPaused;

    private bool interacting;

    private bool locked;

    public CharacterData Data => characterData;

    public CharacterMovement Movement => movement;

    public NeedSystem NeedSystem => needSystem;

    public EmotionSystem EmotionSystem => emotionSystem;

    public bool IsAIPaused => aiPaused;

    public bool IsInteracting => interacting;

    public bool IsLocked => locked;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();

        interaction = GetComponent<CharacterInteraction>();

        needSystem = GetComponent<NeedSystem>();

        emotionSystem = GetComponent<EmotionSystem>();
    }

    private void Start()
    {
        movement.Initialize(characterData.moveSpeed);

        BuildTree();
    }

    private void Update()
    {
        if (!aiPaused)
        {
            behaviourTree.Update();
        }
    }

    public void PauseAI()
    {
        aiPaused = true;
    }

    public void ResumeAI()
    {
        aiPaused = false;
    }

    public void Lock()
    {
        locked = true;

        PauseAI();

        movement.Stop();
    }

    public void Unlock()
    {
        locked = false;

        ResumeAI();
    }

    private void BuildTree()
    {
        NeedNode needNode =
            new NeedNode(needSystem);

        IdleNode idleNode =
            new IdleNode(Random.Range(
                characterData.minIdleTime,
                characterData.maxIdleTime),
                this);

        RandomMoveNode moveNode =
            new RandomMoveNode(
                this,
                movement,
                transform,
                characterData.moveRadius);

        SequenceNode moveSequence =
            new SequenceNode(new List<BTNode>
            {
            idleNode,
            moveNode
            });

        SelectorNode root =
            new SelectorNode(new List<BTNode>
            {
            needNode,
            moveSequence
            });

        behaviourTree =
            new CharacterBehaviourTree(root);
    }

    public void StartInteraction()
    {
        interacting = true;

        PauseAI();

        movement.Stop();
    }

    public void EndInteraction()
    {
        interacting = false;

        ResumeAI();
    }
}
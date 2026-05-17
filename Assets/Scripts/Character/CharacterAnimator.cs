using UnityEngine;
public class CharacterAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [Header("Base Clips")]
    [SerializeField]
    private AnimationClip baseIdle;
    [SerializeField]
    private AnimationClip baseWalk;
    [SerializeField]
    private AnimationClip baseDrag;
    [Header("Emotion Sets")]
    [SerializeField]
    private EmotionAnimationSet normalSet;
    [SerializeField]
    private EmotionAnimationSet happySet;
    [SerializeField]
    private EmotionAnimationSet sadSet;
    [SerializeField]
    private EmotionAnimationSet angrySet;
    [SerializeField]
    private EmotionAnimationSet suprisedSet;
    private AnimatorOverrideController overrideController;
    private CharacterMovement movement;
    private bool facingLeft = true;
    private EmotionType currentEmotion = EmotionType.Normal;
    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        overrideController = new AnimatorOverrideController(
            animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        SetEmotion(EmotionType.Normal);
    }
    private void Update()
    {
        UpdateMovement();
        UpdateDirection();
    }
    private void UpdateMovement()
    {
        animator.SetBool("IsWalking", movement.IsMoving());
    }
    private void UpdateDirection()
    {
        Vector2 direction = movement.GetMoveDirection();
        if (direction.x > 0.01f)
            FaceRight();
        else if (direction.x < -0.01f)
            FaceLeft();
    }

    public void FaceToward(Vector2 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
            FaceRight();
        else
            FaceLeft();
    }

    private void FaceLeft()
    {
        if (facingLeft) return;
        facingLeft = true;
        spriteRenderer.flipX = false;
    }
    private void FaceRight()
    {
        if (!facingLeft) return;
        facingLeft = false;
        spriteRenderer.flipX = true;
    }
    public void SetEmotion(EmotionType emotion)
    {
        currentEmotion = emotion;
        ApplyEmotion(emotion);
    }
    public void SetDragging(bool dragging)
    {
        animator.SetBool("IsDragging", dragging);
        if (dragging)
            ApplyEmotion(EmotionType.Surprised);
        else
            ApplyEmotion(currentEmotion);
    }
    public void ApplyController(RuntimeAnimatorController controller)
    {
        overrideController = new AnimatorOverrideController(controller);
        animator.runtimeAnimatorController = overrideController;
    }
    private void ApplyEmotion(EmotionType emotion)
    {
        EmotionAnimationSet set = GetAnimationSet(emotion);
        overrideController[baseIdle.name] = set.idle;
        overrideController[baseWalk.name] = set.walk;
        overrideController[baseDrag.name] = set.drag;
    }
    private EmotionAnimationSet GetAnimationSet(EmotionType emotion)
    {
        switch (emotion)
        {
            case EmotionType.Happy: return happySet;
            case EmotionType.Sad: return sadSet;
            case EmotionType.Angry: return angrySet;
            case EmotionType.Surprised: return suprisedSet;
        }
        return normalSet;
    }
}
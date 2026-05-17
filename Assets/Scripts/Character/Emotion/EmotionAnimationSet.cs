using UnityEngine;

[CreateAssetMenu(
    menuName =
    "Game/EmotionAnimationSet")]
public class EmotionAnimationSet
    : ScriptableObject
{
    public AnimationClip idle;

    public AnimationClip walk;

    public AnimationClip drag;
}
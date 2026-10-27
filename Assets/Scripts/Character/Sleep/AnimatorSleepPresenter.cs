using UnityEngine;

// Default presenter: drives a bool parameter on an Animator.
// The Animator Controller needs the bool parameter and transitions to/from a sleep clip.
public class AnimatorSleepPresenter : MonoBehaviour, ISleepPresenter
{
    [SerializeField] private Animator animator;
    [SerializeField] private string sleepBoolParameter = "IsSleeping";

    private int sleepHash;

    private void Reset()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        sleepHash = Animator.StringToHash(sleepBoolParameter);
    }

    public void PlaySleep()
    {
        animator.SetBool(sleepHash, true);
    }

    public void PlayWake()
    {
        animator.SetBool(sleepHash, false);
    }
}
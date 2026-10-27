using System;
using UnityEngine;

// Per-character sleep state holder. Attach to each character root.
// Integration points (call from existing input code):
//   NotifyDropped(worldPosition) : when the user releases a drag
//   RequestWake()                : when the user clicks or starts dragging a sleeping character
public class SleepController : MonoBehaviour
{
    [SerializeField] private LayerMask bedLayerMask;
    [Tooltip("Optional. Switched to Kinematic while sleeping so physics does not move the character. Colliders stay active for click detection.")]
    [SerializeField] private Rigidbody2D body;

    private ISleepPresenter presenter;
    private CharacterController controller;
    private Quaternion rotationBeforeSleep;
    private RigidbodyType2D bodyTypeBeforeSleep;

    public bool IsSleeping { get; private set; }

    // Observer: true when falling asleep, false when waking up.
    public event Action<bool> OnSleepStateChanged;

    private void Awake()
    {
        presenter = GetComponentInChildren<ISleepPresenter>();
        controller = GetComponent<CharacterController>();
    }

    public bool NotifyDropped(Vector2 worldPosition)
    {
        if (IsSleeping)
        {
            return false;
        }

        Collider2D hit = Physics2D.OverlapPoint(worldPosition, bedLayerMask);
        if (hit == null)
        {
            return false;
        }

        Bed bed = hit.GetComponentInParent<Bed>();
        if (bed == null || !bed.TryGetAnchor(this, out Transform anchor))
        {
            return false;
        }

        EnterSleep(anchor);
        return true;
    }

    public void RequestWake()
    {
        if (!IsSleeping)
        {
            return;
        }

        ExitSleep();
    }

    private void EnterSleep(Transform anchor)
    {
        rotationBeforeSleep = transform.rotation;

        // Lock() stops movement, pauses the behaviour tree and sets IsLocked.
        if (controller != null)
        {
            RequestWake();
            controller.Lock();
        }

        if (body != null)
        {
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0f;
            bodyTypeBeforeSleep = body.bodyType;
            body.bodyType = RigidbodyType2D.Kinematic;
        }

        Vector3 target = anchor.position;
        target.z = transform.position.z;
        transform.SetPositionAndRotation(target, anchor.rotation);

        IsSleeping = true;
        if (presenter != null)
        {
            presenter.PlaySleep();
        }

        OnSleepStateChanged?.Invoke(true);
    }

    private void ExitSleep()
    {
        transform.rotation = rotationBeforeSleep;

        if (body != null)
        {
            body.bodyType = bodyTypeBeforeSleep;
        }

        IsSleeping = false;

        if (controller != null)
        {
            controller.Unlock();
            NotifyDropped(transform.position);
        }
        if (presenter != null)
        {
            presenter.PlayWake();
        }

        OnSleepStateChanged?.Invoke(false);
    }
}
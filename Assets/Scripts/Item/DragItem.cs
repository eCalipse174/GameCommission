using System.Collections;
using UnityEngine;

public class DragItem : MonoBehaviour
{
    private Camera mainCamera;

    private bool dragging;

    private bool returning;

    private Vector3 offset;

    private Vector3 startPosition;

    [SerializeField]
    private float dropCheckRadius = 1f;

    [SerializeField]
    private float returnDuration = 0.25f;

    [SerializeField]
    private float respawnDelay = 0.5f;

    private Collider2D itemCollider;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        mainCamera = Camera.main;

        itemCollider =
            GetComponent<Collider2D>();

        spriteRenderer =
            GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void OnMouseDown()
    {
        Debug.Log($"{name} OnMouseDown called / returning: {returning} / dragging: {dragging}");
        if (returning)
            return;
        dragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseUp()
    {
        if (!dragging)
            return;

        dragging = false;

        TryDrop();
    }

    private void Update()
    {
        if (!dragging)
            return;

        transform.position =
            GetMouseWorldPosition() + offset;
    }

    private void TryDrop()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                dropCheckRadius);

        foreach (Collider2D hit in hits)
        {
            if (!hit.TryGetComponent(
                out CharacterNeedResolver resolver))
            {
                continue;
            }

            bool success =
                resolver.TryResolve(
                    GetComponent<ItemObject>());

            if (success)
            {
                Consume();

                return;
            }
        }

        ReturnToStartPosition();
    }

    public void ReturnToStartPosition()
    {
        Debug.Log($"{name} ReturnToStartPosition called / returning: {returning}");
        if (returning) return;
        StartCoroutine(SmoothReturnRoutine());
    }

    private IEnumerator SmoothReturnRoutine()
    {
        returning = true;

        Vector3 startPos =
            transform.position;

        float elapsed = 0f;

        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                elapsed / returnDuration;

            t = EaseOutCubic(t);

            transform.position =
                Vector3.Lerp(
                    startPos,
                    startPosition,
                    t);

            yield return null;
        }

        transform.position =
            startPosition;

        returning = false;
    }

    private void Consume()
    {
        StartCoroutine(
            RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        dragging = false;

        itemCollider.enabled = false;

        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(
            respawnDelay);

        transform.position = startPosition;

        itemCollider.enabled = true;

        spriteRenderer.enabled = true;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos =
            Input.mousePosition;

        mousePos.z = 10f;

        return mainCamera
            .ScreenToWorldPoint(mousePos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            dropCheckRadius);
    }

    private float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }
}
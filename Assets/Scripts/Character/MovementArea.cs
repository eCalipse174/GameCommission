using UnityEngine;

public class MovementArea
    : MonoBehaviour
{
    public static MovementArea
        Instance;

    [SerializeField]
    private BoxCollider2D
        areaCollider;

    private Bounds bounds;

    private void Awake()
    {
        Instance = this;

        bounds =
            areaCollider.bounds;
    }

    public Vector2 GetRandomPoint()
    {
        float x =
            Random.Range(
                bounds.min.x,
                bounds.max.x);

        float y =
            Random.Range(
                bounds.min.y,
                bounds.max.y);

        return new Vector2(x, y);
    }

    public Vector2 ClampPosition(
        Vector2 position)
    {
        float x =
            Mathf.Clamp(
                position.x,
                bounds.min.x,
                bounds.max.x);

        float y =
            Mathf.Clamp(
                position.y,
                bounds.min.y,
                bounds.max.y);

        return new Vector2(x, y);
    }

    public bool IsInside(
        Vector2 position)
    {
        return bounds.Contains(position);
    }

    private void OnDrawGizmos()
    {
        if (areaCollider == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            areaCollider.bounds.center,
            areaCollider.bounds.size);
    }
}
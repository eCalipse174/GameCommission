using UnityEngine;

public class MovementArea : MonoBehaviour
{
    public static MovementArea Instance;

    [Header("Mode")]
    [SerializeField] private bool useScreenBoundary;

    [Header("Collider Mode")]
    [SerializeField] private PolygonCollider2D areaCollider;

    [Header("Screen Mode")]
    [SerializeField] private Camera targetCamera;

    [SerializeField] private int randomPointTryCount = 30;

    private IMovementBoundary boundary;

    private void Awake()
    {
        Instance = this;

        if (useScreenBoundary)
        {
            Camera cam = targetCamera != null ? targetCamera : Camera.main;
            boundary = new ScreenMovementBoundary(cam);
        }
        else
        {
            boundary = new ColliderMovementBoundary(areaCollider);
        }
    }

    public Vector2 GetRandomPoint()
    {
        return boundary.GetRandomPoint(randomPointTryCount);
    }

    public Vector2 ClampPosition(Vector2 position)
    {
        return boundary.ClampPosition(position);
    }

    public bool IsInside(Vector2 position)
    {
        return boundary.IsInside(position);
    }

    private void OnDrawGizmos()
    {
        if (useScreenBoundary || areaCollider == null)
            return;

        Gizmos.color = Color.green;
        Vector2[] points = areaCollider.points;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 current = transform.TransformPoint(points[i]);
            Vector2 next = transform.TransformPoint(points[(i + 1) % points.Length]);
            Gizmos.DrawLine(current, next);
        }
    }
}
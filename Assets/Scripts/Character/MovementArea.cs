using UnityEngine;

public class MovementArea
    : MonoBehaviour
{
    public static MovementArea
        Instance;

    [SerializeField]
    private PolygonCollider2D
        areaCollider;

    [SerializeField]
    private int randomPointTryCount = 30;

    private Bounds bounds;

    private void Awake()
    {
        Instance = this;

        bounds =
            areaCollider.bounds;
    }

    public Vector2 GetRandomPoint()
    {
        for (int i = 0;
             i < randomPointTryCount;
             i++)
        {
            float x =
                Random.Range(
                    bounds.min.x,
                    bounds.max.x);

            float y =
                Random.Range(
                    bounds.min.y,
                    bounds.max.y);

            Vector2 point =
                new Vector2(x, y);

            if (IsInside(point))
            {
                return point;
            }
        }

        return areaCollider.bounds.center;
    }

    public Vector2 ClampPosition(
        Vector2 position)
    {
        if (IsInside(position))
        {
            return position;
        }

        Vector2 closest =
            areaCollider.ClosestPoint(
                position);

        return closest;
    }

    public bool IsInside(
        Vector2 position)
    {
        return areaCollider
            .OverlapPoint(position);
    }

    private void OnDrawGizmos()
    {
        if (areaCollider == null)
            return;

        Gizmos.color =
            Color.green;

        Vector2[] points =
            areaCollider.points;

        for (int i = 0;
             i < points.Length;
             i++)
        {
            Vector2 current =
                transform.TransformPoint(
                    points[i]);

            Vector2 next =
                transform.TransformPoint(
                    points[
                        (i + 1) %
                        points.Length]);

            Gizmos.DrawLine(
                current,
                next);
        }
    }
}
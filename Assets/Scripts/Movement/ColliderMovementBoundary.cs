using UnityEngine;

public class ColliderMovementBoundary : IMovementBoundary
{
    private readonly PolygonCollider2D areaCollider;
    private readonly Bounds bounds;

    public ColliderMovementBoundary(PolygonCollider2D collider)
    {
        areaCollider = collider;
        bounds = collider.bounds;
    }

    public Vector2 GetRandomPoint(int tryCount)
    {
        for (int i = 0; i < tryCount; i++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            Vector2 point = new Vector2(x, y);

            if (IsInside(point))
                return point;
        }
        return areaCollider.bounds.center;
    }

    public Vector2 ClampPosition(Vector2 position)
    {
        if (IsInside(position))
            return position;

        return areaCollider.ClosestPoint(position);
    }

    public bool IsInside(Vector2 position)
    {
        return areaCollider.OverlapPoint(position);
    }
}
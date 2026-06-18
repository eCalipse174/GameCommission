using UnityEngine;

public class ScreenMovementBoundary : IMovementBoundary
{
    private readonly Camera worldCamera;

    public ScreenMovementBoundary(Camera camera)
    {
        worldCamera = camera;
    }

    public Vector2 GetRandomPoint(int tryCount)
    {
        Vector2 min = GetWorldMin();
        Vector2 max = GetWorldMax();

        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        return new Vector2(x, y);
    }

    public Vector2 ClampPosition(Vector2 position)
    {
        Vector2 min = GetWorldMin();
        Vector2 max = GetWorldMax();

        float clampedX = Mathf.Clamp(position.x, min.x, max.x);
        float clampedY = Mathf.Clamp(position.y, min.y, max.y);
        return new Vector2(clampedX, clampedY);
    }

    public bool IsInside(Vector2 position)
    {
        Vector2 min = GetWorldMin();
        Vector2 max = GetWorldMax();

        return position.x >= min.x && position.x <= max.x &&
               position.y >= min.y && position.y <= max.y;
    }

    private Vector2 GetWorldMin()
    {
        return worldCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
    }

    private Vector2 GetWorldMax()
    {
        return worldCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
    }
}
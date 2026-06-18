using UnityEngine;

public interface IMovementBoundary
{
    Vector2 GetRandomPoint(int tryCount);
    Vector2 ClampPosition(Vector2 position);
    bool IsInside(Vector2 position);
}
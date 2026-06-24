using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BottomEdgeBackground : MonoBehaviour
{
    [SerializeField] private Camera worldCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int sortingOrder = -1;

    private void Start()
    {
        Camera cam = worldCamera != null ? worldCamera : Camera.main;
        PlaceAtBottom(cam);
    }

    private void PlaceAtBottom(Camera cam)
    {
        spriteRenderer.sortingOrder = sortingOrder;

        Vector2 worldMin = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector2 worldMax = cam.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));

        float screenWidth = worldMax.x - worldMin.x;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;

        // 비율 유지: X, Y 스케일을 동일하게 적용
        float scale = screenWidth / spriteWidth;
        transform.localScale = new Vector3(scale, scale, 1f);

        float centerX = (worldMin.x + worldMax.x) * 0.5f;

        // 피벗이 Bottom-Center -> Y는 화면 맨 아래에 그대로 맞추면 됨
        transform.position = new Vector3(centerX, worldMin.y, 0f);
    }
}
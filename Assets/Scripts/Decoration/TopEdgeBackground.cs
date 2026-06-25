using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TopEdgeBackground : MonoBehaviour
{
    [SerializeField] private Camera worldCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int sortingOrder = -1;

    private void Start()
    {
        Camera cam = worldCamera != null ? worldCamera : Camera.main;
        PlaceAtTop(cam);
    }

    private void PlaceAtTop(Camera cam)
    {
        spriteRenderer.sortingOrder = sortingOrder;

        Vector2 worldMin = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
        Vector2 worldMax = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        float screenWidth = worldMax.x - worldMin.x;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;

        // 비율 유지: X, Y 스케일을 동일하게 적용
        float scale = screenWidth / spriteWidth;
        transform.localScale = new Vector3(scale, scale, 1f);

        float centerX = (worldMin.x + worldMax.x) * 0.5f;

        // 피벗이 Top-Center -> Y는 화면 맨 위에 그대로 맞추면 됨
        transform.position = new Vector3(centerX, worldMin.y, 0f);
    }
}
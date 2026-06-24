using UnityEngine;

public class BottomEdgeRow : MonoBehaviour
{
    [SerializeField] private Camera worldCamera;
    [SerializeField] private EdgeDecoration decorationPrefab;
    [SerializeField] private float yOffset = 0f; // 화면 맨 아래에서 위로 띄울 정도
    [SerializeField] private int sortingOrder = -1; // 캐릭터보다 뒤에 두고 싶으면 음수

    private void Start()
    {
        Camera cam = worldCamera != null ? worldCamera : Camera.main;
        PlaceAlongBottom(cam);
    }

    private void PlaceAlongBottom(Camera cam)
    {
        Vector2 worldMin = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector2 worldMax = cam.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));

        float decorationWidth = GetSpriteWorldWidth(decorationPrefab);
        if (decorationWidth <= 0f) return;

        float totalWidth = worldMax.x - worldMin.x;
        int count = Mathf.CeilToInt(totalWidth / decorationWidth);

        for (int i = 0; i < count; i++)
        {
            float x = worldMin.x + decorationWidth * i + decorationWidth * 0.5f;
            Vector3 spawnPos = new Vector3(x, worldMin.y + yOffset, 0f);

            EdgeDecoration decoration = Instantiate(decorationPrefab, spawnPos, Quaternion.identity, transform);
            decoration.SetSortingOrder(sortingOrder);
        }
    }

    private float GetSpriteWorldWidth(EdgeDecoration prefab)
    {
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return 0f;

        return sr.sprite.bounds.size.x * prefab.transform.localScale.x;
    }
}
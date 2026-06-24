using System.Collections;
using UnityEngine;

public class OutfitDragItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private float dropCheckRadius = 1f;

    private Camera mainCamera;
    private AdultOutfit outfit;
    private OutfitManager targetOutfitManager;
    private bool dragging;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void BeginDrag(AdultOutfit outfit, OutfitManager targetOutfitManager)
    {
        this.outfit = outfit;
        this.targetOutfitManager = targetOutfitManager;
        dragging = true;

        if (iconRenderer != null && outfit.thumbnail != null)
            iconRenderer.sprite = outfit.thumbnail;
    }

    private void Update()
    {
        if (!dragging) return;

        transform.position = GetMouseWorldPosition();

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            TryDrop();
        }
    }

    private void TryDrop()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, dropCheckRadius);

        foreach (Collider2D hit in hits)
        {
            if (!hit.TryGetComponent(out OutfitDropTarget dropTarget))
                continue;

            bool success = dropTarget.TryApplyOutfit(outfit, targetOutfitManager);
            if (success)
            {
                Destroy(gameObject);
                return;
            }
        }

        // 캐릭터 위가 아닌 곳에 드롭 -> 그냥 사라짐 (옷장은 항상 다시 열 수 있음)
        Destroy(gameObject);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, dropCheckRadius);
    }
}
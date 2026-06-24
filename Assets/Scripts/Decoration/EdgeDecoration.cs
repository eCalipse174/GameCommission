using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EdgeDecoration : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Sprite Sprite => spriteRenderer.sprite;

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetSortingOrder(int order)
    {
        spriteRenderer.sortingOrder = order;
    }
}
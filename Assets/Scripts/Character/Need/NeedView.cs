using UnityEngine;

public class NeedView : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer iconRenderer;

    [SerializeField]
    private Sprite hungerIcon;

    [SerializeField]
    private Sprite sleepIcon;

    [SerializeField]
    private Sprite playIcon;

    private void Start()
    {
        Hide();
    }

    public void Show(NeedType type)
    {
        iconRenderer.gameObject.SetActive(true);

        switch (type)
        {
            case NeedType.Hunger:
                iconRenderer.sprite = hungerIcon;
                break;

            case NeedType.Sleep:
                iconRenderer.sprite = sleepIcon;
                break;

            case NeedType.Play:
                iconRenderer.sprite = playIcon;
                break;
        }
    }

    public void Hide()
    {
        iconRenderer.gameObject.SetActive(false);
    }
}
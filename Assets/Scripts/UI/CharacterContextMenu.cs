using System;
using UnityEngine;

public class CharacterContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuRoot;
    [SerializeField] private GameObject fullscreenBarrier;
    [SerializeField] private RectTransform menuRect;

    private CharacterController targetController;
    private Action onCloseCallback;

    public void Open(CharacterController controller, Action onClose)
    {
        targetController = controller;
        onCloseCallback = onClose;

        PlaceMenuAtMouse();
        fullscreenBarrier.SetActive(true);
        menuRoot.SetActive(true);
    }

    // 배리어의 Button OnClick에 이 메서드를 직접 연결
    public void Close()
    {
        menuRoot.SetActive(false);
        fullscreenBarrier.SetActive(false);
        onCloseCallback?.Invoke();
        onCloseCallback = null;
        targetController = null;
    }

    private void PlaceMenuAtMouse()
    {
        RectTransform canvasRect = menuRoot.transform.parent.GetComponent<RectTransform>();
        Canvas canvas = canvasRect.GetComponentInParent<Canvas>();
        Camera canvasCamera = canvas.worldCamera;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, Input.mousePosition, canvasCamera, out localPoint);

        Vector2 menuSize = menuRect.rect.size;
        Vector2 canvasHalfSize = canvasRect.rect.size * 0.5f;

        float pivotX = localPoint.x + menuSize.x > canvasHalfSize.x ? 1f : 0f;
        float pivotY = localPoint.y - menuSize.y < -canvasHalfSize.y ? 0f : 1f;

        menuRect.pivot = new Vector2(pivotX, pivotY);
        menuRect.anchoredPosition = localPoint;
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickResetToBaby()
    {
        if (targetController == null)
        {
            Close();
            return;
        }

        CharacterGrowth growth = targetController.GetComponent<CharacterGrowth>();
        if (growth != null)
        {
            growth.ResetGrowth();
        }

        Close();
    }
}
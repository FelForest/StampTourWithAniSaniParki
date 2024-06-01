using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 initialTouchPosition;
    private Vector2 initialSizeDelta;
    private Vector2 initialAnchoredPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        initialTouchPosition = eventData.position;
        initialSizeDelta = rectTransform.sizeDelta;
        initialAnchoredPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchDelta = eventData.position - initialTouchPosition;

        rectTransform.anchoredPosition = initialAnchoredPosition + touchDelta;
    }
}

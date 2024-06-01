using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 이동 시작 시 초기 위치를 저장합니다.
        initialPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그하는 동안 오브젝트를 이동합니다.
        transform.position = eventData.position;
    }
}

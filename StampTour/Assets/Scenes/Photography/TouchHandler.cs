using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Canvas canvas;
    private Vector3 initialPosition;
    private Camera mainCamera; // 카메라 변수 추가

    private void Awake()
    {
        initialPosition = transform.position;
        mainCamera = Camera.main; // 메인 카메라 할당
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 이동 시작 시 초기 위치를 저장합니다.
        initialPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 터치 좌표를 월드 좌표로 변환하여 오브젝트를 이동합니다.
        Debug.Log("OnDrag");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, Camera.main, out Vector2 pos);
        transform.position = canvas.transform.TransformPoint(pos);


        // newPosition.z = initialPosition.z; // z 축 값 유지
        // transform.position = newPosition;
    }
}

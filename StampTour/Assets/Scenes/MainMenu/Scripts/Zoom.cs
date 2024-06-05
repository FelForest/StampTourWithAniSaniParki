using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float minZoom = 1.0f;
    public float maxZoom = 3f;

    private Vector2 initialTouchDistance;
    private Vector3 initialScale;
    private void Update()
    {
        MouseZoom();
        TouchZoom();
    }

    private void MouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            float newScale = transform.localScale.x + scroll * zoomSpeed;
            newScale = Mathf.Clamp(newScale, minZoom, maxZoom);
            transform.localScale = new Vector2(newScale, newScale);
        }
    }

    private void TouchZoom()
    {
        if (Input.touchCount == 2) // 터치가 두 개인 경우
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // 터치 시작 시 초기 거리와 크기 저장
            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                initialTouchDistance = touch0.position - touch1.position;
                initialScale = transform.localScale;
            }
            // 터치 이동 시 크기 조절
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                Vector2 currentTouchDistance = touch0.position - touch1.position;
                float scaleFactor = currentTouchDistance.magnitude / initialTouchDistance.magnitude;

                Vector3 newScale = initialScale * scaleFactor;

                // 크기 제한 적용
                newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
                newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);

                transform.localScale = newScale;
            }
        }
    }
}

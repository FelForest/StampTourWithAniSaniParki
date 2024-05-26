using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float minZoom = 1.0f;
    public float maxZoom = 3f;

    private bool isTouching = false;
    private float touchDistance;

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            float newScale = transform.localScale.x + scroll * zoomSpeed;
            newScale = Mathf.Clamp(newScale, minZoom, maxZoom);
            transform.localScale = new Vector2(newScale, newScale);
        }

        MobileZoom();
    }

    private void MobileZoom()
    {
        if(Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float currentTouchDistance = Vector2.Distance(touch1.position, touch2.position);
            if (!isTouching)
            {
                touchDistance = currentTouchDistance;

                isTouching = true;
            }

            else
            {
                float deltaDistance = currentTouchDistance - touchDistance;
                AdjustScale(deltaDistance);
                touchDistance = currentTouchDistance;
            }
        }
        else
        {
            isTouching = false;
        }
    }

    private void AdjustScale(float deltaDistance)
    {
        Vector3 currentScale = transform.localScale;

        float scaleDelta = deltaDistance * zoomSpeed;

        currentScale += new Vector3(scaleDelta, scaleDelta, 0f);

        currentScale.x = Mathf.Clamp(currentScale.x, minZoom, maxZoom);
        currentScale.y = Mathf.Clamp(currentScale.y, minZoom, maxZoom);

        transform.localScale = currentScale;
    }
}

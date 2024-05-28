using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float minZoom = 1.0f;
    public float maxZoom = 3f;

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            float newScale = transform.localScale.x + scroll * zoomSpeed;
            newScale = Mathf.Clamp(newScale, minZoom, maxZoom);
            transform.localScale = new Vector2(newScale, newScale);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutCamController : MonoBehaviour
{
    public RawImage RawImage;

    private void OnEnable() {
        RawImage.texture.width = Screen.width;
        RawImage.texture.height = Screen.height;
    }

    // private void Update() {
    //     Debug.Log(RawImage.texture.width);
    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    WebCamTexture camTexture;
    public RawImage cameraViewImage;

    [HideInInspector]
    public bool isCamera = false;
    private void Start()
    {
        CameraOn();
    }
    public void CameraOn()
    {
        Debug.Log("camera on");
        if(!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("no camera");
            return;
        }

        WebCamDevice[] devices = WebCamTexture.devices;
        int selectedCameraIndex = -1;

        for(int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                selectedCameraIndex = i;
                break;
            }
        }

        if(selectedCameraIndex >= 0)
        {
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name);
            camTexture.requestedFPS = 30;
            cameraViewImage.texture = camTexture;
            camTexture.Play();
            isCamera = true;
        }
    }

    public void CameraOff()
    {
        Debug.Log("camera off");
        if (camTexture != null)
        {
            camTexture.Stop();
            WebCamTexture.Destroy(camTexture);
            camTexture = null;
          
        }
    }

    public byte[] GetCameraFrame()
    {
        if (camTexture != null && camTexture.isPlaying)
        {
            Texture2D snap = new Texture2D(camTexture.width, camTexture.height);
            snap.SetPixels(camTexture.GetPixels());
            snap.Apply();
            return snap.EncodeToJPG(); // JPG로 인코딩하여 바이트 배열로 반환
        }
        return null;
    }
}

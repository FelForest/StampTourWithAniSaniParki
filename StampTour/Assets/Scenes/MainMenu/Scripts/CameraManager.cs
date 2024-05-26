using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CameraManager : MonoBehaviour
{
    WebCamTexture camTexture;

    [HideInInspector]
    public bool isCamera = false;
    public RawImage cameraViewImage; //카메라가 보여질 화면
    
    public void CameraOn() //카메라 켜기
    {
        //카메라 권한 확인
        if(!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        if(WebCamTexture.devices.Length == 0) //카메라가 없다면..
        {
            Debug.Log("no camera!");
            return;
        }

        WebCamDevice[] devices = WebCamTexture.devices; //스마트폰의 카메라 정보를 모두 가져옴.
        int selectedCameraIndex = -1;

        //후면 카메라 찾기
        for (int i = 0; i< devices.Length; i++)
        {
            //현재는 휴대폰이 없어서 앞으로 해놓음, 후면 카메라 활성화시 false로 만들것
            if(devices[i].isFrontFacing == true)
            {
                selectedCameraIndex = i;
                break;
            }
        }

        //카메라 켜기
        if(selectedCameraIndex >= 0)
        {
            //선택된 후면 카메라를 가져옴.
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name);
            
            camTexture.requestedFPS = 30; //카메라 프레임설정

            cameraViewImage.texture = camTexture; //영상을 raw Image에 할당.

            camTexture.Play(); // 카메라 시작하기
            isCamera = true;
        }

    }

    public void CameraOff() //카메라 끄기
    {
        if(camTexture != null)
        {
            camTexture.Stop(); //카메라 정지
            WebCamTexture.Destroy(camTexture); //카메라 객체반납
            camTexture = null; //변수 초기화
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
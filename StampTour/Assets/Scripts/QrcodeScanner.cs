using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Android;
using System;

public class QRcodeScanner : MonoBehaviour
{
    [SerializeField]
    private RawImage _rawImageBackground;

    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter;

    [SerializeField]
    private TextMeshProUGUI _textOut;

    [SerializeField]
    private RectTransform _scanZone;

    private bool _isCamAvaible;
    private WebCamTexture _cameraTexture;

    private bool _isCamPlaying;
    private int selectedCameraIndex;

    static Result result;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetUpCamera());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRender();
    }
 
    public void SwitchCamera()
    {
        if (!_isCamAvaible)
        {
            Debug.Log("ī�޶� ����");
            return;
        }

        _isCamPlaying = _cameraTexture.isPlaying;
        Debug.Log(_isCamPlaying);

        if (!_isCamPlaying)
        {
            CameraOn();
        }
        else
        {
            CameraOff();
        }
    }
    private void UpdateCameraRender()
    {
        if(_isCamAvaible == false || _cameraTexture == null)
        {
            return;
        }
        float ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;

        int orientation = -_cameraTexture.videoRotationAngle;
        _rawImageBackground.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }

    
    private IEnumerator SetUpCamera()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.Camera));
        }

        WebCamDevice[] devices;

        do
        {
            devices = WebCamTexture.devices;
            yield return null;
        }
        while (devices.Length != WebCamTexture.devices.Length);




        if(devices.Length == 0)
        {
            _isCamAvaible = false;
            yield break;
        }

        selectedCameraIndex = -1;

        for (int i =0; i<devices.Length; i++)
        {
            Debug.Log(devices[i].name);
            if(devices[i].isFrontFacing == false)
            {
                selectedCameraIndex = i;
                break;
            }
        }

        if (selectedCameraIndex >= 0)
        {
            Debug.Log("ī�޶� �غ� �Ϸ�");
            _cameraTexture = new WebCamTexture(devices[selectedCameraIndex].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
            _isCamAvaible = true;
        }
        else
        {
            Debug.Log("ī�޶� �� �޾ƿ�");
            _isCamAvaible = false;
            yield break;
        }
    }
    
    private void CameraOn()
    {
        _cameraTexture.Play();
        _rawImageBackground.texture = _cameraTexture;
    }
    private void CameraOff()
    {
        _cameraTexture.Stop();
    }
    

    public void OnclickScan()
    {
        Scan();
    }

    private void Scan()
    {
        int sceneNum = -1;
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            result = barcodeReader.Decode(_cameraTexture.GetPixels32(),_cameraTexture.width,_cameraTexture.height);
            if(result != null)
            {
                _textOut.text=result.Text;
                sceneNum = int.Parse(result.Text);
            }
            else
            {
                _textOut.text = "FAILED TO READ QR CODE";
            }
        }
        catch (Exception e)
        {
            _textOut.text = "FAILED IN TRY";
            Debug.LogWarning(e);
        }

        if (sceneNum >= 0)
            GameManager.LoadScene(sceneNum);
        

    }

    public void Reset()
    {
        _rawImageBackground.texture = null;
        _textOut.text = " ";
    }
}


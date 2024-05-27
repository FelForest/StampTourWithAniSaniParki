using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Android;

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



    // Start is called before the first frame update
    void Start()
    {
        SetUpCamera();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRender();
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

    
    public void SetUpCamera()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        WebCamDevice [] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            _isCamAvaible = false;
            return;
        }
        for(int i =0; i<devices.Length; i++)
        {
            if(devices[i].isFrontFacing==false)
            {
                _cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width,(int)_scanZone.rect.height);

            }
        }

        _cameraTexture.Play();
        _rawImageBackground.texture = _cameraTexture;
        _isCamAvaible = true;

    }   

    public void OnclickScan()
    {
        Scan();
    }

    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(),_cameraTexture.width,_cameraTexture.height);
            if(result != null)
            {
                _textOut.text=result.Text;
            }
            else
            {
                _textOut.text = "FAILED TO READ QR CODE";
            }
        }

        catch
        {
            _textOut.text = "FAILED IN TRY";
        }
    }
}


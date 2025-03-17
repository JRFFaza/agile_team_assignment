using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class QRCodeScanner : MonoBehaviour
{

    [SerializeField]
    private RawImage _rawImageBackground; // render what camera sees on phone

    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter;

    [SerializeField]
    private TextMeshProUGUI _textOut;

    [SerializeField]
    private RectTransform _scanZone;

    private bool _isCamAvailable;
    private WebCamTexture _cameraTexture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        SetUpCamera();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRender();
    }

    private void SetUpCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            _isCamAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                _cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
            }
        }

        _cameraTexture.Play();
        _rawImageBackground.texture = _cameraTexture;
        _isCamAvailable = true;
    }

    private void UpdateCameraRender() // take all device
    {
        if (_isCamAvailable == false)
        {
            return;
        }

        float ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;

        int orientation = -_cameraTexture.videoRotationAngle;
        _rawImageBackground.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }

    public void OnClickScan()
    {
        Scan();
    }

    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);
            if (result != null)
            {
                _textOut.text = result.Text;
                if (result.Text == "ARCVFLOOR1" )
                {
                    SceneManager.LoadScene("ARCVFloor1");
                }
                else if (result.Text == "ARCVFLOOR-1")
                {
                    SceneManager.LoadScene("ARCVFloor-1");
                }
                else if (result.Text == "ARCVFLOOR0")
                {
                    SceneManager.LoadScene("ARCVFloor0");
                }
            }
            else
            {
                _textOut.text = "FAILED TO READ QR CODE";
            }
        }
        catch
        {
            _textOut.text = "FAIL IN TRY";
        }
    }
}

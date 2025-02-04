using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ZXing;
using TMPro;
using UnityEngine.UI;

public class QRCodeScript : MonoBehaviour
{
    [SerializeField]
    private RawImage rawIamgeBackground;

    [SerializeField]
    private AspectRatioFitter aspectRatioFitter;

    [SerializeField]
    private TextMeshProUGUI textOutput;

    [SerializeField]
    private RectTransform scanningZone;

    private bool camAvailable;
    private WebCamTexture camTexture;

    void Start()
    {
        SetUpCamera();
    }
    void Update()
    {
        UpdateCameraRender();
    }

    private void SetUpCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            camAvailable = false;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                camTexture = new WebCamTexture(devices[i].name, (int)scanningZone.rect.width, (int)scanningZone.rect.height);
            }
        }
        camTexture.Play();
        rawIamgeBackground.texture = camTexture;
        camAvailable = true;
    }

    private void UpdateCameraRender()
    {
        if (!camAvailable)
        {
            return;
        }
        float ratio = (float)camTexture.width/(float)camTexture.height;
        aspectRatioFitter.aspectRatio = ratio;

        int orientation = -camTexture.videoRotationAngle;
        rawIamgeBackground.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }

    public void OnClickScanQR()
    {

    }

    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
            
            if (result != null)
            {
                textOutput.text = result.Text;
            }
            else
            {
                textOutput.text = "QR Scanning failed";
            }
        }
        catch
        {
            textOutput.text = "Scanning failed";
            throw;
        }
    }
}


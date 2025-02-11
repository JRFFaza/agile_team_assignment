using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ZXing;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Android;

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
    private WebCamTexture webcamTexture;

    void Start()
    {

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        SetUpCamera();
    }
    void Update()
    {
        UpdateCameraRender();
    }

    void OnDisable()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
            Debug.Log("Camera preview stopped.");
        }
    }

    private void SetUpCamera()
    {
        WebCamDevice [] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == true)
            {
                webcamTexture = new WebCamTexture(devices[i].name, (int)scanningZone.rect.width, (int)scanningZone.rect.height);
            }
        }
        camAvailable = true;
        rawIamgeBackground.material = new Material(Shader.Find("Unlit/Texture"));
        rawIamgeBackground.gameObject.SetActive(true);
        rawIamgeBackground.texture = webcamTexture;
        rawIamgeBackground.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    private void UpdateCameraRender()
    {
        rawIamgeBackground.texture = webcamTexture;

        float ratio = (float)webcamTexture.width/(float)webcamTexture.height;
        aspectRatioFitter.aspectRatio = ratio;

        int orientation = webcamTexture.videoRotationAngle;
        rawIamgeBackground.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }

    public void OnClickScanQR()
    {
        Scan();
    }

    private void Scan()
    {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(webcamTexture.GetPixels32(), webcamTexture.width, webcamTexture.height);
            
            if (result != null)
            {
                textOutput.text = result.Text;
            }
            else
            {
                textOutput.text = "QR Scanning failed";
            }
    }
}


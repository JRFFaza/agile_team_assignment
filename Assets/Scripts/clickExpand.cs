using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class clickExpand : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    public GameObject panelObj;

    [SerializeField]
    public TextMeshProUGUI textOut;

    [SerializeField]
    public TextMeshProUGUI textIn;

    [SerializeField]
    public AudioClip clipIn;

    [SerializeField]
    public AudioSource clipOut;
    public void OpenPanel()
    {
        if (panelObj != null)
        {
            textOut.text = textIn.text;
            panelObj.SetActive(true);
        }
    }
    
    public void Speech()
    {
        clipOut.PlayOneShot(clipIn);
    }
}

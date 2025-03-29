using TMPro;
using UnityEngine;

public class clickExpand : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject panelObj;

    [SerializeField]
    public TextMeshProUGUI textOut;

    [SerializeField]
    public TextMeshProUGUI textIn;
    public void OpenPanel()
    {
        if (panelObj != null)
        {
            textOut.text = textIn.text;
            panelObj.SetActive(true);
        }
    }
}

using TMPro;
using UnityEngine;

public class clickExpand : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject panelObj;

    [SerializeField]
    public TextMeshProUGUI textOut;
    public void OpenPanel()
    {
        if (panelObj != null)
        {
            textOut.text = "This is scene 3xxxx";
            panelObj.SetActive(true);
        }
    }
}

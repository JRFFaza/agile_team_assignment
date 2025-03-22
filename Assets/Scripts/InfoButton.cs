using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoButton : MonoBehaviour
{
    public void OpenInfo()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        SceneManager.LoadScene("InfoScene");
    }
}

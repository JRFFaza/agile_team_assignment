using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void StartApp()
    {
        SceneManager.LoadScene("QRJoyce");
    }
}

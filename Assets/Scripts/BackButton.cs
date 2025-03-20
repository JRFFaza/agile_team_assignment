using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void LoadPreviousScene()
    {
        SceneManager.LoadScene("StartScreen"); 
    }
}

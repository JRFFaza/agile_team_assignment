using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void LoadPreviousScene()
    {
        Debug.Log("BackButton clicked.");
        Debug.Log("Stored previousScene: " + SceneHistory.previousScene);

        if (!string.IsNullOrEmpty(SceneHistory.previousScene))
        {
            SceneManager.LoadScene(SceneHistory.previousScene);
        }
        else
        {
            Debug.LogWarning("Previous scene was empty! Loading StartScreen fallback.");
            SceneManager.LoadScene("StartScreen");
        }
    }
}

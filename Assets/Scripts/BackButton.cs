using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void LoadPreviousScene()
    {
        if(!string.IsNullOrEmpty(SceneHistory.previousScene))
        {
            SceneManager.LoadScene(SceneHistory.previousScene);
        }
        else
        {
            SceneManager.LoadScene("StartScreen");
        } 
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneHistory.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}

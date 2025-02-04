using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoButton : MonoBehaviour
{
    /* // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
     {

     }*/

    public void OpenInfo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        //SceneManager.LoadScene("InfoScreen");
    }
}

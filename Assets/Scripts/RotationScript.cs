using Unity.VisualScripting;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject cubeObj;
    void Start()
    {
        cubeObj = GameObject.Find("dummy");
    }

    // Update is called once per frame
    void Update()
    {
        OnMouseDragX();
    }

    void OnMouseDragX()
    {

        float rotationX = Input.GetAxis("Mouse X") * -100 * Mathf.Deg2Rad;
        transform.Rotate(Vector3.down, -rotationX, Space.World);

    }

    void OnMouseDragY()
    {
        float rotationY = Input.GetAxis("MouseY") * -100 * Mathf.Deg2Rad;
        //transform.localScale += Vector3(rotationY,0.1,0.1);
        //cubeObj.gameObject.transform.localScale += new Vector3(0,rotationY,0);
    }
}

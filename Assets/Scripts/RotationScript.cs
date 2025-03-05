using Unity.VisualScripting;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnMouseDrag();
    }

    void OnMouseDrag()
    {

        float rotationX = Input.GetAxis("Mouse X") * -100 * Mathf.Deg2Rad;
        transform.Rotate(Vector3.down, -rotationX, Space.World);

    }
}

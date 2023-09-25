using UnityEngine;

public class BaseSpin : MonoBehaviour
{
    public float rotationSpeed = 45.0f; // Adjust the rotation speed as needed.

    void Update()
    {
        // Rotate the object around its Y-axis.
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}

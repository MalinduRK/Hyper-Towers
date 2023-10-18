using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2.0f;
    public float minSize = 3.0f;
    public float maxSize = 5.0f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the orthographic size based on the scroll input.
        float newSize = mainCamera.orthographicSize - scroll * zoomSpeed;

        // Clamp the size within the specified range.
        newSize = Mathf.Clamp(newSize, minSize, maxSize);

        // Set the new orthographic size.
        mainCamera.orthographicSize = newSize;
    }
}

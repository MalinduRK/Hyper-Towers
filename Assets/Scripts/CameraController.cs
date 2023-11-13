using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2.0f;
    public float minSize = 3.0f;
    public float maxSize = 5.0f;
    public float targetAspectRatio = 1.5f; // The target aspect ratio of the game

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        // Change camera according to resolution
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float cameraSize = mainCamera.orthographicSize;
        CameraDebug($"Screen: {Screen.width} x {Screen.height} | Aspect ratio: {aspectRatio}");

        if (aspectRatio < targetAspectRatio)
        {
            // Screen is taller than the target, adjust the orthographic size
            cameraSize = mainCamera.orthographicSize * (targetAspectRatio / aspectRatio);
        }
        // Doesn't need to adjust size if the screen is wider. The background object will stretch to fit the screen width

        //CameraDebug($"Camera pixel size: {mainCamera.pixelWidth} x {mainCamera.pixelHeight}");
        mainCamera.orthographicSize = cameraSize;

        // Set min and max sizes for zoom function
        maxSize = mainCamera.orthographicSize;
        minSize = maxSize - 2;
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


    [Header("Debug")]

    [SerializeField] private bool cameraDebug;

    public void CameraDebug(string message)
    {
        if (cameraDebug)
        {
            Debug.Log(message);
        }
    }
}

using UnityEngine;

// Attach this script to any background object to scale it with screen size
public class ScaleToScreenSize : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float targetAspectRatio = 1.5f; // The target aspect ratio of the game

    private void Start()
    {
        // Get the current transform values of the object
        float localX = transform.localScale.x;
        float localY = transform.localScale.y;

        // Get current aspect ratio
        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float newWidth;

        if (aspectRatio > targetAspectRatio)
        {
            // Screen is wider than the target, adjust the object size
            newWidth = localY / (aspectRatio / targetAspectRatio);
        }
        else
        {
            // Screen is taller than the target
            newWidth = localY * (targetAspectRatio / aspectRatio);
        }

        // Set the scale to fit the screen
        transform.localScale = new Vector3(localX, newWidth, 1);
    }
}

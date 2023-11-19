using UnityEngine;

public class PointerLight : MonoBehaviour
{
    // For some reason, this position defaults to -10
    private float zPosition = 9f; // Light z position -10

    private void Update()
    {
        // Get the mouse position in screen space
        Vector3 mousePos = Input.mousePosition;

        // Convert the screen space mouse position to world space, keeping the Z position fixed
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zPosition));

        // Set the Point Light's position to the worldPos
        transform.position = worldPos;
    }
}

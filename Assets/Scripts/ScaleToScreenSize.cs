using UnityEngine;

// Attach this script to any background object to scale it with screen size
public class ScaleToScreenSize : MonoBehaviour
{
    private void Start()
    {
        // Get the current transform values of the object
        float localX = transform.localScale.x;
        float localY = transform.localScale.y;

        // Get the screen width and height
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        // Get the sprite's width and height
        float spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        float spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        float scaleX;
        float scaleY;
        // Calculate the scale factor to fit the screen
        if (spriteWidth < screenWidth)
        {
            scaleX = (screenWidth / spriteWidth) * localX;

            if (spriteHeight < screenHeight)
            {
                scaleY = (screenHeight / spriteHeight) * localY;
            }
            else
            {
                scaleY = (spriteHeight / screenHeight) * localY;
            }
        }
        else
        {
            scaleX = (spriteWidth / screenWidth) * localX;

            if (spriteHeight < screenHeight)
            {
                scaleY = (screenHeight / spriteHeight) * localY;
            }
            else
            {
                scaleY = (spriteHeight / screenHeight) * localY;
            }
        }

        // Set the scale to fit the screen
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}

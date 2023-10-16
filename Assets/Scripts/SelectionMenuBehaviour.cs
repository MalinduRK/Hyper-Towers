using UnityEngine;

public class SelectionMenuBehaviour : MonoBehaviour
{
    [Header("Game Objects")]
    private GameInteractivity interactionManager;

    private void Start()
    {
        // Assign interaction manager
        interactionManager = GameObject.Find("InteractionManager").GetComponent<GameInteractivity>();
    }

    private void OnMouseExit()
    {
        // NOTE!!!
        // Below is not the intended behaviour. It is supposed to work when the mouse is NOT over a child object. Find a solution soon

        // Close the menu only if the mouse exited the selection circle range entirely
        if (!IsMouseOverChildObject())
        {
            SMenuDebug("Selection circle closed");

            // Re-enable interactions with outside objects
            interactionManager.EnableInteractions();

            // Destroy selection circle
            Destroy(gameObject);
        }
    }

    // Check if the mouse is now hovering over one of its child objects
    private bool IsMouseOverChildObject()
    {
        // Cast a ray from the mouse position to detect objects.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, LayerMask.GetMask("Default"));

        if (hit.collider != null && hit.collider.transform.IsChildOf(transform))
        {
            SMenuDebug("Mouse is over child object");
            return hit.collider != null && hit.collider.transform.IsChildOf(transform);
        }

        SMenuDebug("Mouse is not over child object");
        return false;
    }

    //--Debugs

    [SerializeField] private bool sMenuDebug;

    private void SMenuDebug(string message)
    {
        if (sMenuDebug)
        {
            Debug.Log(message);
        }
    }
}

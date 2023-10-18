using UnityEngine;

public class SelectionMenuBehaviour : MonoBehaviour
{
    [Header("Game Objects")]
    private GameInteractivity interactionManager;

    [Header("Components")]
    private DetailPanelController detailPanelController;

    private void Start()
    {
        // Assign detail panel controller
        detailPanelController = GameObject.Find("UIManager").GetComponent<DetailPanelController>();

        // Notify other scripts that the selection menu is open
        detailPanelController.isSelectionMenuOpen = true;

        // Assign interaction manager
        interactionManager = GameObject.Find("InteractionManager").GetComponent<GameInteractivity>();
    }

    private void OnMouseExit()
    {
        // Close the menu only if the mouse exited the selection circle range entirely
        if (!IsMouseOverChildObject())
        {
            SMenuDebug("Selection circle closed");

            // Re-enable interactions with outside objects
            interactionManager.EnableInteractions();

            // Notify other scripts that the selection menu is closed
            detailPanelController.isSelectionMenuOpen = false;

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

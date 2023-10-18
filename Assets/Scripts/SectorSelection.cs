using Unity.VisualScripting;
using UnityEngine;

// This script is responsible for handling the selection of a sector in the selection circle

public class SectorSelection : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject relatedObject; // Game object related with the selection circle
    private GameObject parentCircle; // Parent selection circle

    [Header("Components")]
    public Sprite objectSprite;
    private ScrapCounter scrapCounter;
    private GameInteractivity interactionManager;
    private DetailPanelController detailPanelController;

    [Header("Variables")]
    public string selectionCategory; // Narrows down the selection circle to check in which situation the circle is in (tower built/not built)
    public string assignedAction; // Action assigned for the sector

    private void Start()
    {
        // Get the parent Selection Circle of the sector
        parentCircle = gameObject.transform.parent.gameObject;

        // Find scrap manager and assign scrapCounter
        GameObject scrapManager = GameObject.Find("ScrapManager");
        scrapCounter = scrapManager.GetComponent<ScrapCounter>();

        // Assign interaction manager
        interactionManager = GameObject.Find("InteractionManager").GetComponent<GameInteractivity>();

        // Assign detail panel controller
        detailPanelController = GameObject.Find("UIManager").GetComponent<DetailPanelController>();
    }

    private void OnMouseEnter()
    {
        // Higlight sector

        // Pass tower reference and sprite to detail panel controller
        detailPanelController.referenceObjectName = assignedAction;
        detailPanelController.referenceSprite = objectSprite;

        // Notify detail panel controller that the mouse is hovering over a sector
        detailPanelController.isHoveringOverSector = true;
    }

    private void OnMouseExit()
    {
        // Notify detail panel controller that the mouse is not hovering over a sector
        detailPanelController.isHoveringOverSector = false;
    }

    private void OnMouseDown() // Clicking on a sector
    {
        // Switch statement is carried out in nested format to minimize the search time for the correct action
        switch (selectionCategory)
        {
            case "Towers": // Build tower
                relatedObject.GetComponent<TowerPlacement>().BuildTower(assignedAction);
                break;

            case "Manage": // Manage tower (upgrade/raze)
                // Nested switch here to check with assigned action
                break;
        }

        // Destroy selection circle
        Destroy(parentCircle);

        // Re-enable interactions with outside objects
        interactionManager.EnableInteractions();

        // Notify other scripts that the selection menu is closed
        detailPanelController.isSelectionMenuOpen = false;
    }
}

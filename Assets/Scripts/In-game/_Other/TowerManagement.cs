using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerManagement : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Sprite upgradeIcon;
    [SerializeField] private Sprite sellIcon;

    [Header("Game Objects")]
    [SerializeField] private GameObject towerRangePrefab;
    [SerializeField] private GameObject selectionCirclePrefab;
    private GameInteractivity interactionManager;

    [Header("Components")]
    private DetailPanelController detailPanelController;

    private void Start()
    {
        // Assign interaction manager
        interactionManager = GameObject.Find("InteractionManager").GetComponent<GameInteractivity>();

        // Assign detail panel controller
        detailPanelController = GameObject.Find("UIManager").GetComponent<DetailPanelController>();
    }

    public void SelectTower()
    {
        // Create new selection circle
        GameObject selectionCircle = Instantiate(selectionCirclePrefab, transform.parent);

        // Disable interactions with outside objects
        interactionManager.DisableInteractions();


        //--Top Sector

        /*
        // Reference selection sectors within the selection circle
        GameObject selectionSectorTop = selectionCircle.transform.Find("SelectionSectorTop").gameObject;

        // Assign icon sprite of the relevent function to the Icon sprite of the current sector
        selectionSectorTop.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = upgradeIcon;

        // Assign item references to selection sector
        SectorSelection sectorSelectionTop = selectionSectorTop.GetComponent<SectorSelection>();
        sectorSelectionTop.relatedObject = gameObject;
        sectorSelectionTop.selectionCategory = "manage";
        sectorSelectionTop.assignedAction = "upgrade";
        sectorSelectionTop.objectSprite = upgradeIcon;
        */


        //--Top Sector

        // Disable top sector for level 0
        if (SceneManager.GetActiveScene().name != "Level0" )
        {
            // Reference selection sectors within the selection circle
            GameObject selectionSectorTop = selectionCircle.transform.Find("SelectionSectorTop").gameObject;

            // Assign icon sprite of the relevent function to the Icon sprite of the current sector
            selectionSectorTop.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = upgradeIcon;

            // Assign item references to selection sector
            SectorSelection sectorSelectionTop = selectionSectorTop.GetComponent<SectorSelection>();
            sectorSelectionTop.relatedObject = gameObject;
            sectorSelectionTop.selectionCategory = "manage";
            sectorSelectionTop.assignedAction = "upgrade";
            sectorSelectionTop.objectSprite = upgradeIcon;
        }

        //--Bottom Sector

        // Reference selection sectors within the selection circle
        GameObject selectionSectorBottom = selectionCircle.transform.Find("SelectionSectorBottom").gameObject;

        // Assign icon sprite of the relevent function to the Icon sprite of the current sector
        selectionSectorBottom.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = sellIcon;

        // Assign item references to selection sector
        SectorSelection sectorSelectionBottom = selectionSectorBottom.GetComponent<SectorSelection>();
        sectorSelectionBottom.relatedObject = gameObject;
        sectorSelectionBottom.selectionCategory = "manage";
        sectorSelectionBottom.assignedAction = "raze";
        sectorSelectionBottom.objectSprite = sellIcon;

        // Display detail panel
        ShowDetailPanel();
    }

    private void ShowDetailPanel()
    {
        // Pass tower reference and sprite to detail panel controller
        detailPanelController.referenceObjectName = gameObject.GetComponent<TowerStats>().towerData.id;
        detailPanelController.tierId = gameObject.GetComponent<TowerStats>().currentTier;
        detailPanelController.referenceSprite = gameObject.GetComponent<SpriteRenderer>().sprite;

        // Notify detail panel controller that the mouse is hovering over the tower
        detailPanelController.isHoveringOverTower = true;
    }
}

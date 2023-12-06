using UnityEngine;

public class TowerManagement : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Sprite upgradeIcon;
    [SerializeField] private Sprite sellIcon;

    [Header("Game Objects")]
    [SerializeField] private GameObject towerRangePrefab;
    [SerializeField] private GameObject selectionCirclePrefab;
    private GameInteractivity interactionManager;

    private void Start()
    {
        // Assign interaction manager
        interactionManager = GameObject.Find("InteractionManager").GetComponent<GameInteractivity>();
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
    }
}

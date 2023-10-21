using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private AudioClip turretBuild;

    [Header("Game Objects")]
    [SerializeField] private GameObject towerRangePrefab;
    [SerializeField] private GameObject[] towerPrefabs; // Array containing all tower prefabs
    [SerializeField] private GameObject selectionCirclePrefab;
    private GameInteractivity interactionManager;
    private GameObject towerPlotHighlight; // Outline of the tower plot

    [Header("Components")]
    private ScrapCounter scrapCounter;
    private AudioSource audioSource;

    [Header("Variables")]
    private bool towerBuilt = false; // Bool to mark if a tower is built on the plot
    private int towerCost = 5; // Cost of a tower

    private void Start()
    {
        // Find the children object by its name, relative to the parent.
        towerPlotHighlight = transform.Find("TowerPlotHighlight").gameObject;
        towerPlotHighlight.SetActive(false);

        // Find scrap manager and assign scrapCounter
        GameObject scrapManager = GameObject.Find("ScrapManager");
        scrapCounter = scrapManager.GetComponent<ScrapCounter>();

        // Hide tower range object if it is found in plot at the start of the game
        Transform childTransform = transform.Find("TowerRange(Clone)");
        if (childTransform != null)
        {
            // Hide the range ring rather than disabling it
            childTransform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        // Assign interaction manager
        interactionManager = GameObject.Find("InteractionManager").GetComponent<GameInteractivity>();

        // Assign audio source
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnMouseEnter()
    {
        // Check if the TowerPlots object has more than one child
        int childCount = gameObject.transform.childCount;

        if (childCount > 1) // There is a tower on the plot
        {
            towerBuilt = true; // In this context, it means there is a pre-built tower in the plot in this level
            // Show tower range object if it is found in plot
            Transform childTransform = transform.Find("TowerRange(Clone)");
            if (childTransform != null)
            {
                childTransform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                HoverDebug("Showing tower range indicator");
            }
        }
        else // There is no tower on the plot
        {
            // Highlight tower plot
            towerPlotHighlight.SetActive(true);
            HoverDebug("Hovering over tower plot");
        }
    }

    private void OnMouseExit()
    {
        // Hide tower range object if it is found in plot
        Transform childTransform = transform.Find("TowerRange(Clone)");
        if (childTransform != null)
        {
            childTransform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            HoverDebug("Hidden tower range indicator");
        }

        towerPlotHighlight.SetActive(false);
        HoverDebug("Exited hovering over tower plot");
    }

    // Mouse click event
    private void OnMouseDown()
    {
        if (!towerBuilt) // If no tower is built on the plot, build a tower
        {
            // TODO: Rather than instantiating a new selection circle, it should be available whenever a plot is created on the scene, just like the plot highlight

            // Create new selection circle
            GameObject selectionCircle = Instantiate(selectionCirclePrefab, transform);

            // Disable interactions with outside objects
            interactionManager.DisableInteractions();

            // Reference selection sectors within the selection circle
            GameObject selectionSectorTop = selectionCircle.transform.Find("SelectionSectorTop").gameObject;

            //GameObject tower1Icon = Instantiate(towerPrefabs[0], selectionSectorTop.transform);

            // Find the sprite of the displayed game object
            Sprite towerSprite = towerPrefabs[0].GetComponent<SpriteRenderer>().sprite;

            // Assign sprite of the relevent object to the Icon sprite of the current sector
            selectionSectorTop.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = towerSprite;

            // Assign item references to selection sectors
            SectorSelection sectorSelection = selectionSectorTop.GetComponent<SectorSelection>();
            sectorSelection.relatedObject = gameObject;
            sectorSelection.selectionCategory = "Towers";
            sectorSelection.assignedAction = "Tower1";
            sectorSelection.objectSprite = towerSprite;
        }
    }

    public void BuildTower(string towerId)
    {
        // Assign default prefab
        GameObject towerPrefab = towerPrefabs[0];

        // Get the tower info related to the tower to be built
        foreach(GameObject tower in towerPrefabs)
        {
            if (tower.name ==  towerId)
            {
                // Assing correct tower prefab to build
                towerPrefab = tower;
            }
        }

        if (scrapCounter.GetScrap() >= towerCost) // There is enough scrap to build a tower
        {
            // Build new tower
            GameObject newTower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
            newTower.transform.SetParent(transform);
            // Add range prefab
            GameObject newRange = Instantiate(towerRangePrefab, transform.position, Quaternion.identity);
            newRange.transform.SetParent(transform);

            // Hide range prefab after creating
            newRange.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            // Use scrap
            scrapCounter.UseScrap(towerCost);

            BuildDebug("Tower built");

            towerBuilt = true;

            // Play build sound
            audioSource.clip = turretBuild;
            audioSource.Play();
        }
        else // Not enough scrap
        {
            BuildDebug("Not enough scrap to build");
        }
    }

    // TODO: Create tower range object whenever a new tower is created
    /*
    // Create tower range object

    // Instantiate a new tower range object from the prefab
    GameObject newRange = Instantiate(towerRangePrefab, transform.position, Quaternion.identity);
    // Set the parent of the new range object to the tower plot
    newRange.transform.parent = gameObject.transform;

    // Destroy tower range with the tower

    // Find and delete tower range object if its found in plot
    Transform childTransform = transform.Find("TowerRange(Clone)");
    if (childTransform != null)
    {
        // Destroy the child object.
        Destroy(childTransform.gameObject);
        HoverDebug("Destroyed tower range indicator");
    }
     */

    //--Debugs
    [Header("Debugs")]
    [SerializeField] private bool hoverDebug;
    [SerializeField] private bool buildDebug;

    private void HoverDebug(string message)
    {
        if (hoverDebug)
        {
            Debug.Log(message);
        }
    }

    private void BuildDebug(string message)
    {
        if (buildDebug)
        {
            Debug.Log(message);
        }
    }
}

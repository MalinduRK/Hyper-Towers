using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private AudioClip[] turretBuild;
    [SerializeField] private AudioClip cannotBuild;
    [SerializeField] private AudioClip openMenu;
    [SerializeField] private GameObject buildParticles;

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
    public bool towerBuilt = false; // Bool to mark if a tower is built on the plot
    private Vector3 buildPosition; // Corrected build position for turrets

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

        // Set turret position slightly above the tower plot, at the level of enemies
        Vector3 originalPosition = transform.position;
        originalPosition.z -= 0.01f; // Set new z axis
        buildPosition = originalPosition;
    }

    private void OnMouseEnter()
    {
        if (towerBuilt) // There is a tower on the plot
        {
            Transform towerTransform = transform.Find("Tower"); // Find tower
            Transform childTransform = towerTransform.Find("TowerRange"); // Find tower range of tower

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
        if (towerBuilt)
        {
            // Hide tower range object if it is found in plot
            Transform towerTransform = transform.Find("Tower"); // Find tower
            Transform childTransform = towerTransform.Find("TowerRange"); // Find tower range of tower

            if (childTransform != null)
            {
                childTransform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                HoverDebug("Hidden tower range indicator");
            }
        }

        towerPlotHighlight.SetActive(false);
        HoverDebug("Exited hovering over tower plot");
    }

    // Mouse click event
    private void OnMouseUpAsButton()
    {
        if (!towerBuilt) // If no tower is built on the plot, open tower build menu
        {
            // Play menu open sound
            audioSource.clip = openMenu;
            audioSource.Play();

            // TODO: Rather than instantiating a new selection circle, it should be available whenever a plot is created on the scene, just like the plot highlight

            // Create new selection circle
            GameObject selectionCircle = Instantiate(selectionCirclePrefab, transform);

            // Disable interactions with outside objects
            interactionManager.DisableInteractions();

            // Reference selection sectors within the selection circle
            GameObject selectionSectorTop = selectionCircle.transform.Find("SelectionSectorTop").gameObject;

            // Find the sprite of the displayed game object
            Sprite towerSprite = towerPrefabs[0].GetComponent<SpriteRenderer>().sprite;

            // Assign sprite of the relevent object to the Icon sprite of the current sector
            selectionSectorTop.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = towerSprite;

            // Assign item references to selection sectors
            SectorSelection sectorSelection = selectionSectorTop.GetComponent<SectorSelection>();
            sectorSelection.relatedObject = gameObject;
            sectorSelection.selectionCategory = "towers";
            sectorSelection.assignedAction = "Tower1";
            sectorSelection.objectSprite = towerSprite;

            // TODO: Assign references to each tower as above
        }
        else // A tower is built, open tower menu
        {
            // Get reference to the TowerManagement script inside the Tower object
            TowerManagement towerManager = gameObject.transform.Find("Tower").GetComponent<TowerManagement>();

            // Open tower menu
            towerManager.SelectTower();
        }
    }

    public void BuildTower(string towerId)
    {
        // Get data related to the tower
            //
        // Assign data reader
        GameObject dataReaderObject = GameObject.Find("DataManager");
        DataReader dataReader = dataReaderObject.GetComponent<DataReader>();

        // Read and assign tower data
        TowerData towerData = new TowerData();
        towerData = dataReader.ReadTowerData(towerId);

        // Assign default prefab for initializing
        GameObject towerPrefab = towerPrefabs[0];

        // Get the tower info related to the tower to be built
        foreach(GameObject tower in towerPrefabs)
        {
            if (tower.name ==  towerId)
            {
                // Assign correct tower prefab to build
                towerPrefab = tower;
            }
        }

        // Access tier 1 tower data
        int towerCost = towerData.tiers[0].cost; // Cost of the tower

        if (scrapCounter.GetScrap() >= towerCost) // There is enough scrap to build a tower
        {
            // Build new tower
            GameObject newTower = Instantiate(towerPrefab, buildPosition, Quaternion.identity);
            newTower.transform.SetParent(transform);
            // Rename tower object
            newTower.name = "Tower";
            // Set starting scrap value of the tower
            newTower.GetComponent<TowerStats>().scrapValue = towerData.tiers[0].cost;
            // Set starting tier data
            newTower.GetComponent<TowerStats>().tierData = towerData.tiers[0];

            // Add range prefab
            GameObject newRange = Instantiate(towerRangePrefab, transform.position, Quaternion.identity);
            newRange.transform.SetParent(newTower.transform);
            // Rename range object
            newRange.name = "TowerRange";

            // Hide range prefab after creating
            newRange.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            // Set correct range
            float towerRange = towerData.tiers[0].range;
            newRange.transform.localScale = towerRange * Vector3.one;

            // Send tower data to TowerStats.cs
            newTower.gameObject.GetComponent<TowerStats>().towerData = towerData;

            // Use scrap
            scrapCounter.UseScrap(towerCost);

            BuildDebug("Tower built");

            towerBuilt = true;

            // Play build sound
            int randNum = Random.Range(0,2); // Pick a random build sound from the array
            audioSource.clip = turretBuild[randNum];
            audioSource.Play();

            // Play build particle effect
            // Instantiate particle system at the tower's position
            GameObject particleSystem = Instantiate(buildParticles, transform.position, Quaternion.identity);
            // Get the main module of the Particle System
            ParticleSystem.MainModule mainModule = buildParticles.GetComponent<ParticleSystem>().main;
            // Destroy the explosionPrefab after the duration of the Particle System
            Destroy(particleSystem, mainModule.duration);
        }
        else // Not enough scrap
        {
            // Play error sound
            audioSource.clip = cannotBuild;
            audioSource.Play();

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

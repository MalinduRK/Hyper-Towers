using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    //--Variables
    private bool towerBuilt = false; // Bool to mark if a tower is built on the plot
    private int towerCost = 5; // Cost of a tower
    //--Game objects
    public GameObject towerRangePrefab;
    public GameObject towerPrefab;
    private GameObject towerPlotHighlight; // Child object
    //--Components
    private ScrapCounter scrapCounter;

    private void Start()
    {
        // Find the children object by its name, relative to the parent.
        towerPlotHighlight = transform.Find("TowerPlotHighlight").gameObject;
        towerPlotHighlight.SetActive(false);

        // Find scrap manager and assign scrapCounter
        GameObject scrapManager = GameObject.Find("ScrapManager");
        scrapCounter = scrapManager.GetComponent<ScrapCounter>();

        // Hide tower range object if it is found in plot
        Transform childTransform = transform.Find("TowerRange(Clone)");
        if (childTransform != null)
        {
            // Hide the range ring rather than disabling it
            childTransform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //HoverDebug("Hidden tower range indicator");
        }
        else
        {
            //Debug.Log("Tower range not found");
        }
    }

    void OnMouseEnter()
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
            else
            {
                Debug.Log("Tower range not found");
            }
        }
        else // There is no tower on the plot
        {
            // Highlight tower plot
            towerPlotHighlight.SetActive(true);
            HoverDebug("Hovering over tower plot");
        }
    }

    void OnMouseExit()
    {
        // Hide tower range object if it is found in plot
        Transform childTransform = transform.Find("TowerRange(Clone)");
        if (childTransform != null)
        {
            childTransform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            HoverDebug("Hidden tower range indicator");
        }
        else
        {
            Debug.Log("Tower range not found");
        }

        towerPlotHighlight.SetActive(false);
        HoverDebug("Exited hovering over tower plot");
    }

    // Mouse click event
    private void OnMouseDown()
    {
        if (!towerBuilt) // If no tower is built on the plot, build a tower
        {
            if (scrapCounter.GetScrap() >= towerCost) // There is enough scrap to build a tower
            {
                // Build new tower
                GameObject newTower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
                newTower.transform.SetParent(transform);
                // Add range prefab
                GameObject newRange = Instantiate(towerRangePrefab, transform.position, Quaternion.identity);
                newRange.transform.SetParent(transform);

                // Use scrap
                scrapCounter.UseScrap(towerCost);

                BuildDebug("Tower built");
            }
            else // Not enough scrap
            {
                BuildDebug("Not enough scrap to build");
            }
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
    public bool hoverDebug;
    public bool buildDebug;

    void HoverDebug(string message)
    {
        Debug.Log(message);
    }

    void BuildDebug(string message)
    {
        Debug.Log(message);
    }
}

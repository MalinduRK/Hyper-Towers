using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    //--Game objects
    public GameObject towerRangePrefab;
    private GameObject towerPlotHighlight; // Child object

    private void Start()
    {
        // Find the children object by its name, relative to the parent.
        towerPlotHighlight = transform.Find("TowerPlotHighlight").gameObject;
        towerPlotHighlight.SetActive(false);

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

    void HoverDebug(string message)
    {
        Debug.Log(message);
    }
}

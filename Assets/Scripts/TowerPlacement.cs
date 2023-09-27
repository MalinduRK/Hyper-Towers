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
    }

    void OnMouseEnter()
    {
        // Check if the TowerPlots object has more than one child
        int childCount = gameObject.transform.childCount;

        if (childCount > 1) // There is a tower on the plot
        {
            // Show tower radius
            // Instantiate a new tower range object from the prefab
            GameObject newRange = Instantiate(towerRangePrefab, transform.position, Quaternion.identity);
            // Set the parent of the new range object to the tower plot
            newRange.transform.parent = gameObject.transform;
            HoverDebug("Showing tower range");
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
        // Find and delete tower range object if its found in plot
        Transform childTransform = transform.Find("TowerRange(Clone)");
        if (childTransform != null)
        {
            // Destroy the child object.
            Destroy(childTransform.gameObject);
            HoverDebug("Destroyed tower range indicator");
        }
        else
        {
            Debug.Log("Tower range not found");
        }

        towerPlotHighlight.SetActive(false);
        HoverDebug("Exited hovering over tower plot");
    }

    //--Debugs
    public bool hoverDebug;

    void HoverDebug(string message)
    {
        Debug.Log(message);
    }
}

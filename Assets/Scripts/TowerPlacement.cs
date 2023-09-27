using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private GameObject towerPlotHighlight; // Child object

    private void Start()
    {
        // Find the child object by its name, relative to the parent.
        towerPlotHighlight = transform.Find("TowerPlotHighlight").gameObject;
        towerPlotHighlight.SetActive(false);
    }
    void OnMouseEnter()
    {
        towerPlotHighlight.SetActive(true);
        HoverDebug("Hovering over tower plot");
    }

    void OnMouseExit()
    {
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

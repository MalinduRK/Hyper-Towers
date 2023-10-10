using UnityEngine;

// This script handles the interactions with game objects (clicks, button presses and hovers)
public class GameInteractivity : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject towerPlotsParent;
    private Transform[] towerPlots;

    public static bool interactionsDisabled = false; // This variable is useful for mouse enter/exit events

    private void Start()
    {
        // Get all children objects inside the TowerPlots
        towerPlots = new Transform[towerPlotsParent.transform.childCount];
        for (int i = 0; i < towerPlots.Length; i++)
        {
            towerPlots[i] = towerPlotsParent.transform.GetChild(i);
        }
    }

    public void DisableInteractions()
    {
        interactionsDisabled = true;

        foreach (Transform towerPlot in towerPlots)
        {
            // Get polygon collider and disable it
            towerPlot.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    public void EnableInteractions()
    {
        interactionsDisabled = false;

        foreach (Transform towerPlot in towerPlots)
        {
            // Get polygon collider and disable it
            towerPlot.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }
}

using TMPro;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    //--Variables
    public float moveSpeed = 2f; // Adjust the movement speed as needed.
    private int currentWaypointIndex = 0; // Index of the current waypoint.
    //--Game objects
    private Transform waypointsParent; // Reference to the parent GameObject holding waypoints.
    private Transform[] waypoints;   // Array to store waypoints.
    private TextMeshProUGUI baseHP; // Base HP text

    // Start is called before the first frame update
    void Start()
    {
        waypointsParent = GameObject.Find("Waypoints").transform; // Find Waypoints parent object

        if (waypointsParent == null)
        {
            Debug.LogError("Waypoints GameObject not found. Make sure it's named 'Waypoints'.");
            return;
        }

        // Get all child waypoints and store them in the array.
        waypoints = new Transform[waypointsParent.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = waypointsParent.GetChild(i);
        }

        // Find base HP text
        baseHP = GameObject.Find("BaseHPValueText").GetComponent<TextMeshProUGUI>();
        if (baseHP == null)
        {
            Debug.LogError("Base HP text object not found. Make sure it's named 'BaseHPValueText'.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if the enemy has reached the current waypoint.
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Move to the next waypoint.
                currentWaypointIndex++;
            }
        }
        else // All waypoints reached
        {
            // Destroy enemy
            Destroy(gameObject);
            // Get current base HP
            int baseHPValue = int.Parse(baseHP.text);
            // Reduce base HP value by 1
            baseHP.text = (baseHPValue - 1).ToString();
        }
    }
}

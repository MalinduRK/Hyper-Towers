using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [Header("Game Objects")]
    private Transform waypointsParent; // Reference to the parent GameObject holding waypoints.
    private Transform[] waypoints;   // Array to store waypoints.

    [Header("Variables")]
    private float moveSpeed = 2f; // Adjust the movement speed as needed.
    private int currentWaypointIndex = 0; // Index of the current waypoint.

    private void Start()
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
    }

    private void Update()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if the enemy has reached the current waypoint.
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                // Move to the next waypoint.
                currentWaypointIndex++;
            }
        }
        else // Reached base
        {
            // Call BaseBreach function on BaseController
            BaseController baseController = GameObject.Find("Base").GetComponent<BaseController>();
            baseController.BaseBreach();
            // Destroy enemy
            Destroy(gameObject);
        }
    }
}

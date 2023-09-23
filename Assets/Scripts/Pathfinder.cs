using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    //--Variables
    public float moveSpeed = 2f; // Adjust the movement speed as needed.
    private int currentWaypointIndex = 0; // Index of the current waypoint.
    //--Game objects
    private Transform waypointsParent; // Reference to the parent GameObject holding waypoints.
    private Transform[] waypoints;   // Array to store waypoints.

    // Start is called before the first frame update
    void Start()
    {
        // Find the "Waypoints" GameObject by name.
        waypointsParent = GameObject.Find("Waypoints").transform;

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
        else
        {
            // All waypoints reached, you can destroy the enemy or take other actions here.
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    //--Varaibles
    private string enemyTag = "Enemy"; // Tag used by enemy objects
    private Vector3 enemyPos; // Position of the last detected enemy
    private bool enemyInRange = false;
    //--Game objects
    private GameObject enemies;
    private GameObject towerRangeObject; // Tower range
    private GameObject targetEnemy; // The current target of the tower
    //--Components
    private CircleCollider2D towerRangeCollider;

    void Start()
    {
        enemies = GameObject.Find("Enemies");
        // Find the tower range object within the parent
        towerRangeObject = transform.parent.Find("TowerRange(Clone)").gameObject;
        // Get a reference to the 2D collider of the tower range
        towerRangeCollider = towerRangeObject.GetComponent<CircleCollider2D>();
        // Trigger the below function right from the start (0th second) and every 0.5 seconds (2 times per second).
        // Using this is more lightweight on the hardware than using the update method, which runs at least 60 times a second.
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Set the tower to target the enemy nearest to the mouse pointer when holding RMB

        /*
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Mouse position
        Vector3 lookDirection = mousePos - transform.position; // Difference between mouse and tower
        lookDirection.z = 0; // Ensure the object stays in the same plane.

        // Calculate the rotation to look at the mouse pointer.
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f; // Calculate the angle between the direction vector and the X-axis, and then convert it (rad value) to degrees
        // Adding a -90f makes it so that the barrel of the tower is pointing towards the target
        */

        if (enemyInRange) // Look at the enemy
        {
            // Get the position of the current target
            enemyPos = targetEnemy.transform.position;
            Vector3 lookDirection = enemyPos - transform.position; // Difference between enemy and tower
            lookDirection.z = 0; // Ensure the object stays in the same plane.

            // Calculate the rotation to look at the enemy
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f; // Calculate the angle between the direction vector and the X-axis, and then convert it (rad value) to degrees
            // Adding a -90f makes it so that the barrel of the tower is pointing towards the target

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Apply the rotation to the object.
            transform.rotation = rotation;
        }
    }

    public LayerMask detectionLayer; // Assign the layer(s) you want to detect in the Inspector.
    //public float detectionRadius = 2.0f; // Set the detection radius in the Inspector.

    void UpdateTarget()
    {
        /*// Get all enemy objects (with Enemy tag) colliding with the tower range collider
        // Create a ContactFilter2D to filter by layer and tag.
        ContactFilter2D filter = new();
        filter.SetLayerMask(LayerMask.GetMask(enemyTag));

        // Create an array to store the colliders of overlapping objects.
        Collider2D[] colliders = new Collider2D[10]; // You can adjust the size as needed.

        // Perform the overlap check.
        int colliderCount = Physics2D.OverlapCollider(towerRangeCollider, filter, colliders);

        Debug.Log(colliderCount);

        // Iterate through the colliders to access the game objects.
        for (int i = 0; i < colliderCount; i++)
        {
            GameObject enemy = colliders[i].gameObject;

            // You can perform actions on the 'enemy' object here.
            Debug.Log("Found enemy: " + enemy.name);
        }*/

        if (towerRangeCollider == null)
        {
            Debug.LogError("CircleCollider2D component not found on the object.");
            return;
        }

        // Use the radius of the CircleCollider2D for detection.
        float detectionRadius = towerRangeCollider.radius;

        // Detect all objects within the detection radius.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

        // Loop through the detected colliders.
        foreach (Collider2D collider in colliders)
        {
            // Check if the detected collider is not the same as the current object's collider.
            if (collider.gameObject != gameObject)
            {
                enemyInRange = true; // Tell the tower to start targeting
                targetEnemy = collider.gameObject; // Set this enemy as the current target
                // Handle the detection of a new object here.
                Debug.Log("Detected object: " + collider.gameObject.name);
            }
        }
    }
}

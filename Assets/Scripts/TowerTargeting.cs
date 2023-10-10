using Unity.Mathematics;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject targetEnemy; // The current target of the tower
    private GameObject enemies;
    private GameObject towerRangeObject; // Tower range
    private GameObject baseObject;

    [Header("Components")]
    private CircleCollider2D towerRangeCollider;
    [SerializeField] private LayerMask detectionLayer; // Assign the layer(s) you want to detect in the Inspector.

    [Header("Variables")]
    public bool enemyInRange = false;
    private string enemyTag = "Enemy"; // Tag used by enemy objects
    private Vector3 enemyPos; // Position of the last detected enemy
    private Vector3 basePos; // Position of the base
    //public float detectionRadius = 2.0f; // Set the detection radius in the Inspector.

    private void Start()
    {
        // Find the Base object and get its position
        baseObject = GameObject.Find("Base");
        basePos = baseObject.transform.position;
        // Ignore the z position since the Base object is 3D and rotates
        basePos.z = 0;

        enemies = GameObject.Find("Enemies");
        // Find the tower range object within the parent
        towerRangeObject = transform.parent.Find("TowerRange(Clone)").gameObject;
        // Get a reference to the 2D collider of the tower range
        towerRangeCollider = towerRangeObject.GetComponent<CircleCollider2D>();
        // Trigger the below function right from the start (0th second) and every 0.5 seconds (2 times per second).
        // Using this is more lightweight on the hardware than using the update method, which runs at least 60 times a second.
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    private void Update()
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

        if (enemyInRange && (targetEnemy != null)) // Look at the enemy
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

    private void UpdateTarget()
    {
        // Use the radius of the CircleCollider2D for detection.
        //float detectionRadius = towerRangeCollider.radius;

        // Detect all objects within the detection radius.
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

        // Get the local radius of the CircleCollider2D.
        float localRadius = towerRangeCollider.radius;

        // Convert the local radius to world space.
        float worldRadius = transform.TransformVector(Vector2.right * localRadius).magnitude; // This has to be converted like this in order to get an actual radius value rather than relative to the parent

        // Detect all objects within the worldRadius.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, worldRadius, detectionLayer);

        if (colliders.Length == 0) // Do not proceed if no enemies detected in range
        {
            enemyInRange = false;
            return;
        }

        // Variable to store the shortest distance to the base
        float shortestDistance = math.INFINITY; // This is as far as a distance could go, so any enemy in the game is closer than that

        // Loop through the detected colliders.
        foreach (Collider2D collider in colliders)
        {
            // Check if the detected collider is not the same as the current object's collider.
            if (collider.gameObject != gameObject)
            {
                TargetDebug("Detected object: " + collider.gameObject.name);

                GameObject enemyInView = collider.gameObject; // The current enemy in calculation

                // Calculate the distance from enemy to the base
                Vector3 newEnemyPos = enemyInView.transform.position;
                float distanceToBase = Vector3.Distance(newEnemyPos, basePos);
                if (distanceToBase < shortestDistance) // Target this enemy only if it is has the shorter distance out of all enemies in range
                {
                    shortestDistance = distanceToBase;
                    enemyInRange = true; // Tell the tower to start targeting
                    targetEnemy = enemyInView; // Set this enemy as the current target
                }
            }
        }
    }

    //--Debugs
    [Header("Debugs")]
    [SerializeField] private bool targetDebug;

    private void TargetDebug(string message)
    {
        if (targetDebug)
        {
            Debug.Log(message);
        }
    }
}



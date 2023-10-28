using UnityEngine;

public class ProjectileFlight : MonoBehaviour
{
    [Header("Game Objects")]
    private GameObject targetEnemy;

    [Header("Components")]
    private ProjectileStats projectileStats; // ProjectileStats script attached to this projectile

    [Header("Variables")]
    private Vector3 enemyPos;

    private void Start()
    {
        // Get reference to ProjectileStats.cs script
        projectileStats = GetComponent<ProjectileStats>();
    }

    private void Update()
    {
        // Set the target enemy
        targetEnemy = projectileStats.targetEnemy;

        if (targetEnemy != null)
        {
            // Get the position of the target
            enemyPos = targetEnemy.transform.position;
            Vector3 lookDirection = enemyPos - transform.position; // Difference between enemy and bullet
            lookDirection.z = 0; // Ensure the bullet stays in the same plane.

            // Calculate the rotation to look at the enemy
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg; // Calculate the angle between the direction vector and the X-axis, and then convert it (rad value) to degrees

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Apply the rotation to the object.
            transform.rotation = rotation;

            // Move projectile towards the enemy
            MoveTowardsTarget();
        }
        else // Destroy bullet
        {
            Destroy(gameObject);
        }
    }

    private void MoveTowardsTarget()
    {
        // Calculate the step based on speed and time.
        float step = projectileStats.velocity * Time.deltaTime;

        // Move the bullet towards the target position using lerp.
        transform.position = Vector3.MoveTowards(transform.position, enemyPos, step);

        // Check if the bullet has reached the target.
        if (Vector3.Distance(transform.position, enemyPos) < 0.3f)
        {
            EnemyHitDebug("Enemy hit");
            // Trigger the hit function of EnemyStats script
            targetEnemy.GetComponent<EnemyStats>().Hit(projectileStats.damage);
            // Perform actions when the bullet reaches the target.
            // Example: Hit the target or destroy the bullet.
            Destroy(gameObject);
        }
    }

    //--Debugs
    [Header("Debugs")]
    [SerializeField] private bool enemyHitDebug;

    private void EnemyHitDebug(string message)
    {
        if (enemyHitDebug)
        {
            Debug.Log(message);
        }
    }
}

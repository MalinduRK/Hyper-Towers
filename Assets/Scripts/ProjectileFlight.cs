using UnityEngine;

public class ProjectileFlight : MonoBehaviour
{
    //--Variables
    private Vector3 enemyPos;
    private float projectileVelocity = 2.5f; // Bullet speed
    //--Game objects
    public GameObject targetEnemy;

    void Update()
    {
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

    void MoveTowardsTarget()
    {
        // Calculate the step based on speed and time.
        float step = projectileVelocity * Time.deltaTime;

        // Move the bullet towards the target position using lerp.
        transform.position = Vector3.MoveTowards(transform.position, enemyPos, step);

        // Check if the bullet has reached the target.
        if (Vector3.Distance(transform.position, enemyPos) < 0.1f)
        {
            EnemyHitDebug("Enemy hit");
            // Trigger the hit function of EnemyStats script
            targetEnemy.GetComponent<EnemyStats>().Hit();
            // Perform actions when the bullet reaches the target.
            // Example: Hit the target or destroy the bullet.
            Destroy(gameObject);
        }
    }

    //--Debugs

    public bool enemyHitDebug;

    void EnemyHitDebug(string message)
    {
        if (enemyHitDebug)
        {
            Debug.Log(message);
        }
    }
}

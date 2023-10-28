using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private TextAsset towerJson; // Reference to tower_data.json file

    [Header("Game Objects")]
    [SerializeField] private GameObject bulletPrefab;
    private GameObject projectilesParent; // Empty game object which holds all the created projectiles

    [Header("Components")]
    private TowerTargeting towerTargeting;
    private AudioSource laserShot;

    [Header("Variables")]
    private float firerate = 1f; // Fire rate of the tower (lower the faster)
    private float nextLoadTime; // The next time a bullet is loaded into the magazine
    private bool ammoLoaded = false; // The tower has to have this true in order to fire a shot

    private void Start()
    {
        // Get a reference to the TowerTargeting script
        towerTargeting = GetComponent<TowerTargeting>();

        // Find game objects in scene
        projectilesParent = GameObject.Find("Projectiles");

        if (projectilesParent == null )
        {
            Debug.LogError("'Projectiles' object not found in scene");
        }

        // TODO: This is not the final intended code. Should implement a reload function that reloads the magazine after every few shots. The reload function should run here instead.

        // Specify the next load at the start
        nextLoadTime = Time.time + firerate;

        // Get the AudioSource component attached to this GameObject.
        laserShot = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Time.time >= nextLoadTime) // Load a bullet
        {
            ammoLoaded = true;
        }

        if (towerTargeting.enemyInRange && ammoLoaded) // Shoot if an enemy is in range and an ammo is loaded
        {
            ShootBullet();
            // Empty the bullet
            ammoLoaded = false;
            // Start reloading
            nextLoadTime = Time.time + firerate;
        }
    }

    private void ShootBullet()
    {
        // Generate a random pitch value within a desired range.
        float randomPitch = Random.Range(0.95f, 1.05f); // Adjust the range as needed.

        // Set the pitch to the random value.
        laserShot.pitch = randomPitch;

        // Play the audio clip.
        laserShot.Play();

        // Instantiate new bullet at the tower
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Set the target enemy on the newly instantiated bullet
        newBullet.GetComponent<ProjectileStats>().targetEnemy = towerTargeting.targetEnemy;

        // Read the tower stats from TowerStats.cs and pass them to the ProjectileStats.cs script
        TowerData towerData = GetComponent<TowerStats>().towerData;
        newBullet.GetComponent<ProjectileStats>().towerData = towerData;

        // Set the parent of the bullet to the projectilesParent.
        newBullet.transform.parent = projectilesParent.transform;
    }
}

using System;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    //--Variables
    private float firerate = 2f; // Fire rate of the tower
    private float nextLoadTime; // The next time a bullet is loaded into the magazine
    private bool ammoLoaded = false; // The tower has to have this true in order to fire a shot
    //--Game objects
    public GameObject bulletPrefab;
    private GameObject projectilesParent; // Empty game object which holds all the created projectiles
    //--Components
    private TowerTargeting towerTargeting;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the TowerTargeting script
        towerTargeting = GetComponent<TowerTargeting>();

        // Find game objects in scene
        projectilesParent = GameObject.Find("Projectiles");

        if (projectilesParent == null )
        {
            Debug.LogError("'Projectiles' object not found in scene");
        }

        // Load ammo when the tower is first placed
        // TODO: This is not the final intended code. Should implement a reload function that reloads the magazine after every few shots. The reload function should run here instead.
        ammoLoaded = true;

        // Specify the next load at the start
        nextLoadTime = Time.time + firerate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextLoadTime) // Load a bullet
        {
            ammoLoaded = true;
            nextLoadTime = Time.time + firerate;
        }

        if (towerTargeting.enemyInRange && ammoLoaded) // Shoot if an enemy is in range and an ammo is loaded
        {
            ShootBullet();
            // Empty the bullet
            ammoLoaded = false;
        }
    }

    private void ShootBullet()
    {
        // Instantiate new bullet at the tower
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Set the target enemy on the newly instantiated bullet
        newBullet.GetComponent<ProjectileFlight>().targetEnemy = towerTargeting.targetEnemy; // Assign the enemy target to the bullet

        // Set the parent of the bullet to the projectilesParent.
        newBullet.transform.parent = projectilesParent.transform;
    }
}

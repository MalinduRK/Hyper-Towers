using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //--Variables
    public float spawnInterval = 2f;
    //--Game objects
    public GameObject enemyPrefab; // Assign the "Enemy1" prefab in the Inspector.
    public GameObject hpPrefab; // Enemy health bar
    //--Components
    public Transform enemiesParent; // Assign the "Enemies" GameObject in the Inspector.

    private void Start()
    {
        // Start the spawning coroutine.
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Instantiate a new enemy from the prefab.
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            // Set the parent of the spawned enemy to the enemiesParent.
            newEnemy.transform.parent = enemiesParent;

            // Create enemy health bar
            GameObject newHealthBar = Instantiate(hpPrefab, newEnemy.transform.position, Quaternion.identity);
            // Offset of the health bar is set in the prefab itself

            // Attach the health bar to the enemy object so that it also moves with the enemy
            newHealthBar.transform.parent = newEnemy.transform;

            // Wait for the specified interval before spawning the next enemy.
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

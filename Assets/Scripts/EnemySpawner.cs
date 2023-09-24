using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign the "Enemy1" prefab in the Inspector.
    public Transform enemiesParent; // Assign the "Enemies" GameObject in the Inspector.
    public float spawnInterval = 2f;

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
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            // Wait for the specified interval before spawning the next enemy.
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

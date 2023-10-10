using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject hpPrefab; // Enemy health bar
    [SerializeField] private GameObject[] enemies; // Array containing prefabs for all enemies
    [SerializeField] private GameObject waveManager;

    [Header("Components")]
    [SerializeField] private Transform enemiesParent; // Assign the "Enemies" GameObject in the Inspector

    public void SpawnEnemies(WaveData waveData)
    {
        // Spawn enemies according to its pattern
        switch (waveData.pattern)
        {
            case "single":
                StartCoroutine(Single(waveData));
                break;
        }
    }

    // Spawn a single enemy for the entire wave
    private IEnumerator Single(WaveData wave)
    {
        GameObject enemyPrefab = enemies[0]; // Current enemy prefab (Assign default for initialization)

        // Get the correct enemy prefab from the enemies array
        foreach (GameObject enemy in enemies)
        {
            // Find the enemy prefab matching the json data
            if (enemy.name == wave.enemies[0]) // Only the first entry is checked since this is a Single pattern
            {
                enemyPrefab = enemy;
            }
        }

        if (enemyPrefab != null) // Run the code only if the enemy prefab is found
        {
            for (int i = 0; i < wave.enemy_count; i++) // Loop spawner until all enemies are spawned
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
                yield return new WaitForSeconds(wave.spawn_interval);
            }

            Debug.Log("Track enemies");
            StartCoroutine(TrackEnemies());
        }
        else
        {
            Debug.LogError("Enemy prefab not found");

            yield break;
        }
    }

    // Function to track the remaining enemies of a wave
    private IEnumerator TrackEnemies()
    {
        Debug.Log("Start tracking enemies");
        // Check if there are any enemies remaining inside the enemiesParent
        int enemyCount = enemiesParent.transform.childCount;
        Debug.Log(enemyCount);

        while (enemyCount > 0) // There are remaining enemies
        {
            // Wait for one second before checking again to ensure that a wave doesn't end mid-wave
            yield return new WaitForSeconds(1f);
            enemyCount = enemiesParent.transform.childCount;
            Debug.Log(enemyCount);
        }

        // There are no remaining enemies
        Debug.Log("Ending wave");
        EndWave();

        // End coroutine
        yield break;
    }

    private void EndWave()
    {
        Debug.Log("Calling wave controller");
        // Run game end check on WaveController.cs
        WaveController waveController = waveManager.GetComponent<WaveController>();
        waveController.CheckGameEnd();
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject hpPrefab; // Enemy health bar
    [SerializeField] private GameObject[] enemies; // Array containing prefabs for all enemies
    [SerializeField] private GameObject enemyCounter; // Object with text component showing the remaining number of enemies
    [SerializeField] private GameObject spawnerLight; // Light source
    [SerializeField] private GameObject hoverText; // Text that appears when hovering over the object

    [Header("Game Managers")]
    [SerializeField] private GameObject waveManager;
    [SerializeField] private GameObject uiManager;
    [SerializeField] private GameObject stateManager;

    [Header("Components")]
    [SerializeField] private Transform enemiesParent; // Assign the "Enemies" GameObject in the Inspector
    private Animator _animator;
    private TextMeshProUGUI enemyCountText; // Text component of enemyCounter

    [Header("Variables")]
    public bool waveOngoing = false; // Boolean to show whether a wave is ongoing
    private Vector3 spawnPosition; // Corrected spawn position for enemies

    private void Start()
    {
        // Set enemy spawn position slightly below the enemy spawner, at the level of waypoints
        Vector3 originalSpawn = transform.position;
        originalSpawn.z += 0.01f; // Set new z axis
        spawnPosition = originalSpawn;

        // Hide enemy count when starting the game
        enemyCountText = enemyCounter.GetComponent<TextMeshProUGUI>();
        enemyCountText.text = "";

        // Assign animator component
        _animator = spawnerLight.GetComponent<Animator>();

        // Hide hover text
        hoverText.SetActive(false);
    }

    private void OnMouseEnter()
    {
        // Don't display text when there is an ongoing wave
        if (!waveOngoing)
        {
            // Show hover text
            hoverText.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (!waveOngoing)
        {
            // Hide hover text
            hoverText.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (!waveOngoing)
        {
            stateManager.GetComponent<GameState>().StartWave();
        }
    }

    public void SpawnEnemies(WaveData waveData)
    {
        // Disable animation for spawner
        DisableAnimations();

        waveOngoing = true;

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
        // Display total number of enemies this wave
        int enemiesLeft = wave.enemy_count;
        enemyCountText.text = enemiesLeft.ToString();

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
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                // Set the parent of the spawned enemy to the enemiesParent.
                newEnemy.transform.parent = enemiesParent;

                // Create enemy health bar
                GameObject newHealthBar = Instantiate(hpPrefab, newEnemy.transform.position, Quaternion.identity);
                // Offset of the health bar is set in the prefab itself

                // Attach the health bar to the enemy object so that it also moves with the enemy
                newHealthBar.transform.parent = newEnemy.transform;

                // Update number of enemies left to spawn
                enemyCountText.text = (--enemiesLeft).ToString();

                // Wait for the specified interval before spawning the next enemy.
                yield return new WaitForSeconds(wave.spawn_interval);
            }

            EnemyCountDebug("All enemies spawned");
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
        //Debug.Log("Start tracking enemies");
        // Check if there are any enemies remaining inside the enemiesParent
        int enemyCount = enemiesParent.transform.childCount;
        EnemyCountDebug("Enemies left" + enemyCount);

        while (enemyCount > 0) // There are remaining enemies
        {
            // Wait for one second before checking again to ensure that a wave doesn't end mid-wave
            yield return new WaitForSeconds(1f);
            enemyCount = enemiesParent.transform.childCount;
            EnemyCountDebug("Enemies left" + enemyCount);
        }

        // There are no remaining enemies
        WaveEndDebug("Ending wave");
        EndWave();

        // End coroutine
        yield break;
    }

    private void EndWave()
    {
        //Debug.Log("Wave end");
        // Run game end check on WaveController.cs
        WaveController waveController = waveManager.GetComponent<WaveController>();
        waveController.CheckGameEnd();

        EnableAnimations();
        enemyCountText.text = "";
        waveOngoing = false;

        // Convert play button to pause button
        uiManager.GetComponent<PlayButtonController>().EndWave();
    }

    public void EnableAnimations()
    {
        _animator.enabled = true;
    }

    public void DisableAnimations()
    {
        _animator.enabled = false;
    }

    //--Debugs

    [Header("Debugs")]
    public bool enemyCountDebug;
    public bool waveEndDebug;

    private void EnemyCountDebug(string message)
    {
        if (enemyCountDebug)
        {
            Debug.Log(message);
        }
    }

    private void WaveEndDebug(string message)
    {
        if (waveEndDebug)
        {
            Debug.Log(message);
        }
    }
}

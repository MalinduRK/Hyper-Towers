using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveController : MonoBehaviour
{
    //--Assets
    public TextAsset waveJson; // Reference to wave_data.json file
    //--Game objects
    public TextMeshProUGUI waveText; // Current wave
    public TextMeshProUGUI baseHealthText; // Base HP
    public GameObject enemySpawnerObject;
    public GameObject enemiesParent; // Parent class holding all enemy objects
    //--Game managers
    public GameObject notificationManager;
    public GameObject stateManager;
    //--Components
    private EnemySpawner enemySpawner; // Enemy spawner script

    //--Variables
    private int waveId = 0; // Current wave
    private List<WaveData> waveData; // Wave data
    private bool isFinalWave = false; // Check whether if its the final wave

    void Start()
    {
        // Access the name of the current level (scene)
        Scene currentScene = SceneManager.GetActiveScene(); // Get the currently active scene
        string sceneName = currentScene.name;

        // Extract the number from the scene name using regular expressions.
        Match match = Regex.Match(sceneName, @"\d+");

        int currentLevel; // Pre-initialized to avoid issues

        if (match.Success)
        {
            // Convert the matched string to an integer.
            currentLevel = int.Parse(match.Value);

            // Now you have the isolated scene number in the 'currentLevel' variable.
            Debug.Log("Level: " + currentLevel);
        }
        else
        {
            // Handle the case where a number was not found in the scene name.
            Debug.LogError("Level number not found in scene name");

            return;
        }

        // Deserialize the JSON data into a C# object.
        JsonData jsonData = JsonUtility.FromJson<JsonData>(waveJson.text);

        // Access data specific to the current level
        LevelData levelData = jsonData.levels[currentLevel];

        // Access wave data for the current level
        waveData = levelData.waves;

        // Get reference to EnemySpawner component in enemySpawner object
        enemySpawner = enemySpawnerObject.GetComponent<EnemySpawner>();

        // Access the data, e.g., accessing wave1 of level0.
        /*
        foreach (WaveData wave in waves)
        {
            Debug.Log("Enemy Count: " + wave.enemy_count);
            Debug.Log("Spawn Interval: " + wave.spawn_interval);
            // Access other wave properties as needed.
        }
        */
    }

    public void StartNewWave()
    {
        // Check if there are any enemies remaining inside the enemiesParent
        int enemyCount = enemiesParent.transform.childCount;

        // Start wave only if there are no enemies remaining in the scene
        if ((waveId < waveData.Count) && (waveData[waveId] != null) && (enemyCount == 0)) // Handle exceptions
        {
            WaveData currentWave = waveData[waveId++]; // Wave id starts from 0, but the 0th wave is called wave 1, therefore the post increment

            // Check if this is the last wave
            if (waveId < waveData.Count) // Do not use a null check. It throws an error
            {
                isFinalWave = true;

                // Notify final wave to player
                NotificationController notificationController = notificationManager.GetComponent<NotificationController>();
                notificationController.FinalWaveNotifier();
            }

            // Update text
            waveText.text = $"{waveId}";
            // Trigger wave
            enemySpawner.SpawnEnemies(currentWave);
        }
        else // Finished last wave
        {
            Debug.LogWarning("Cannot start new wave");
        }
    }

    // This function runs only after all the enemies of a wave are defeated
    public void CheckGameEnd()
    {
        if (isFinalWave) // This is the final wave
        {
            // Get remaining base HP
            int baseHP = int.Parse(baseHealthText.text);

            // Victory is achieved only if there is at least 1HP remaining. Otherwise Pathfinder.cs script will handle the game over
            if (baseHP > 0)
            {
                GameState gameState = stateManager.GetComponent<GameState>();
                gameState.Victory();
            }
        }
        else if (waveId + 1 < waveData.Count) // Next wave is the final wave
        {
            isFinalWave = true;
            WaveDebug("Get ready for the final wave");

            // Notify final wave to player
            NotificationController notificationController = notificationManager.GetComponent<NotificationController>();
            notificationController.FinalWaveNotifier();
        }
        else // This or the next wave isn't the final wave
        {
            WaveDebug("End of wave");
            // Notify player that the wave has ended
            NotificationController notificationController = notificationManager.GetComponent<NotificationController>();
            notificationController.NextWaveNotifier();
        }
    }

    //--Debugs
    public bool waveDebug;

    private void WaveDebug(string message)
    {
        if (waveDebug)
        {
            Debug.Log(message);
        }
    }
}

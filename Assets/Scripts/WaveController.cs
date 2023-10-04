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
    public GameObject enemySpawnerObject;
    //--Components
    private EnemySpawner enemySpawner; // Enemy spawner script

    //--Variables
    private int waveId = 0; // Current wave
    private List<WaveData> waveData; // Wave data

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
        // Get current wave data
        WaveData currentWave = waveData[waveId++];
        // Update text
        waveText.text = $"{waveId}";
        // Trigger wave
        enemySpawner.SpawnEnemies(currentWave);
    }
}

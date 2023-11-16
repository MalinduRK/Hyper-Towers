using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveController : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private TextAsset waveJson; // Reference to wave_data.json file
    [SerializeField] private AudioClip callWave;
    [SerializeField] private AudioClip callFinalWave;
    [SerializeField] private AudioClip waveClear;
    [SerializeField] private AudioClip waveCrystalClear; // Call when a wave ends with no damage to the base

    [Header("Game Objects")]
    [SerializeField] private TextMeshProUGUI waveText; // Current wave
    [SerializeField] private TextMeshProUGUI baseHealthText; // Base HP
    [SerializeField] private GameObject enemySpawnerObject;
    [SerializeField] private GameObject enemiesParent; // Parent class holding all enemy objects
    //--Game managers
    [SerializeField] private GameObject stateManager;
    [SerializeField] private GameObject dataManager;

    [Header("Components")]
    private EnemySpawner enemySpawner; // Enemy spawner script
    private AudioSource audioSource;

    [Header("Variables")]
    public int waveId = 0; // Current wave
    private List<WaveData> waveData; // Wave data
    private bool isFinalWave = false; // Check whether if its the final wave

    private void Start()
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

        // Assign DataReader.cs
        DataReader dataReader = dataManager.GetComponent<DataReader>();

        // Access wave data for the current level
        waveData = dataReader.ReadWaveData(currentLevel);

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

        // Assign audio source
        audioSource = GetComponent<AudioSource>();
    }

    public void StartNewWave()
    {
        // Check if there are any enemies remaining inside the enemiesParent
        int enemyCount = enemiesParent.transform.childCount;

        // Start wave only if there are no enemies remaining in the scene
        if ((waveId < waveData.Count) && (enemyCount == 0)) // Handle exceptions
        {
            WaveData currentWave = waveData[waveId]; 

            // Update text
            waveText.text = $"{waveId + 1}"; // Wave id starts from 0, but the 0th wave is wave 1
            // Trigger wave
            enemySpawner.SpawnEnemies(currentWave);

            // Play audio and display notification
            if (!isFinalWave) // Normal wave
            {
                // Notify player that the nth wave has started
                NotificationController notificationController = stateManager.GetComponent<NotificationController>();
                notificationController.NextWaveNotifier(waveId + 1);

                audioSource.volume = 1f;
                audioSource.clip = callWave;
                audioSource.Play();
            }
            else // Final wave
            {
                audioSource.volume = 0.2f;
                audioSource.clip = callFinalWave;
                audioSource.Play();

                // Notify player that the final wave has started
                NotificationController notificationController = stateManager.GetComponent<NotificationController>();
                notificationController.FinalWaveNotifier();
            }
        }
        else // Finished last wave
        {
            Debug.LogWarning("Cannot start new wave");
        }
    }

    // This function runs only after all the enemies of a wave are defeated
    public void CheckGameEnd()
    {
        Debug.Log("Checking game end");
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
        else if (waveId + 2 == waveData.Count) // Next wave is the final wave
        {
            isFinalWave = true;
            WaveDebug("Get ready for the final wave");

            // Notify player that the wave has ended
            NotificationController notificationController = stateManager.GetComponent<NotificationController>();
            notificationController.WaveCompleteNotifier();

            // Play audio
            audioSource.volume = 0.5f;
            audioSource.clip = waveClear;
            audioSource.Play();
        }
        else // This or the next wave isn't the final wave
        {
            WaveDebug("End of wave");
            // Notify player that the wave has ended
            NotificationController notificationController = stateManager.GetComponent<NotificationController>();
            notificationController.WaveCompleteNotifier();

            // Play audio
            audioSource.volume = 0.5f;
            audioSource.clip = waveClear;
            audioSource.Play();
        }

        // Prepare for next wave
        waveId++;
    }

    //--Debugs
    [Header("Debugs")]
    [SerializeField] private bool waveDebug;

    private void WaveDebug(string message)
    {
        if (waveDebug)
        {
            Debug.Log(message);
        }
    }
}

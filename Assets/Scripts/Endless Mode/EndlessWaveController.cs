using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndlessWaveController : MonoBehaviour
{
    [Header("Assets")]
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
    private WaveData waveData; // Wave data
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

        // Set first wave data
        waveData = new WaveData
        {
            enemy_count = 5,
            spawn_interval = 2.0f,
            enemies = new string[] { "Enemy1" },
            pattern = "single"
        };

        // Get reference to EnemySpawner component in enemySpawner object
        enemySpawner = enemySpawnerObject.GetComponent<EnemySpawner>();

        // Assign audio source
        audioSource = GetComponent<AudioSource>();
    }

    public void StartNewWave()
    {
        // Check if there are any enemies remaining inside the enemiesParent
        int enemyCount = enemiesParent.transform.childCount;

        // Start wave only if there are no enemies remaining in the scene
        if (enemyCount == 0) // Handle exceptions
        {
            WaveData currentWave = CreateNewWave(waveId); 

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

    // Use this function to create new waves, infinitely
    private WaveData CreateNewWave(int waveId)
    {
        WaveData previousWaveData = waveData;

        // Change stats of the next wave
        waveData = new WaveData
        {
            enemy_count = previousWaveData.enemy_count + 5,
            spawn_interval = previousWaveData.spawn_interval - 0.1f,
            enemies = previousWaveData.enemies,
            pattern = previousWaveData.pattern
        };

        return waveData;
    }

    // This function runs only after all the enemies of a wave are defeated
    public void CheckGameEnd()
    {
        WaveDebug("End of wave");
        // Notify player that the wave has ended
        NotificationController notificationController = stateManager.GetComponent<NotificationController>();
        notificationController.WaveCompleteNotifier();

        // Play audio
        audioSource.volume = 0.5f;
        audioSource.clip = waveClear;
        audioSource.Play();

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

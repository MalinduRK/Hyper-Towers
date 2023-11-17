using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GameState : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip escapeMenuOpenSound;
    [SerializeField] private AudioClip escapeMenuCloseSound;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Game Objects")]
    [SerializeField] private GameObject overlayPanel;
    [SerializeField] private GameObject escapeMenuPanel;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private GameObject escapeButtonPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject tutorialPanel;

    [Header("Game Managers")]
    [SerializeField] private GameObject waveManager;
    [SerializeField] private GameInteractivity interactionManager;
    [SerializeField] private GameObject uiManager;
    [SerializeField] private GameObject settingsManager;

    [Header("Components")]
    private InterfaceAudioHandler interfaceAudioManager; // Persistent audio manager
    private MusicController musicManager; // Persistent audio manager
    private SettingsReaderWriter settingsReaderWriter;

    [Header("Variables")]
    private bool isPaused = true;
    private bool isEscaped = false; // Turns true when escape menu is open
    private bool isGameOver = false; // This turns true when the game ends in a victory or defeat
    private bool spacebarEnabled = true; // This controls whether the spacebar resumes/pauses the game or not
    private bool escapeEnabled = true; // This controls whether the esc key opens/closes the escape menu
    private float lowpassCutoff = 350f; // Cutoff frequency for the music lowpass filter
    private float lowpassCutoffDefault = 22000f; // Default cutoff frequency for the music lowpass filter

    private void Start()
    {
        // Disable overlay panel
        overlayPanel.SetActive(false);

        // Disable escape menu
        escapeMenuPanel.SetActive(false);

        // Find audio managers
        interfaceAudioManager = GameObject.Find("PersistentAudioManager").GetComponent<InterfaceAudioHandler>();
        musicManager = GameObject.Find("PersistentAudioManager").GetComponent<MusicController>();

        // Assign settingsReaderWriter
        settingsReaderWriter = settingsManager.GetComponent<SettingsReaderWriter>();

        // Open tutorial panel on start if it hadn't been seen by the player before
        //interactionManager.DisableInteractions(); // Prevent player from playing the game until tutorial panel is closed
        StartCoroutine(StartTutorial()); // Open tutorial after a slight delay to let the player see the game
    }

    private void Update()
    {
        // Escape key check
        if (Input.GetKeyDown(KeyCode.Escape) && escapeEnabled)
        {
            if (isEscaped) // Already in escape menu. Resume game
            {
                CloseEscapeMenu();
            }
            else // Escape
            {
                OpenEscapeMenu();
            }
        }

        // Check if the spacebar key is pressed
        if (Input.GetKeyDown(KeyCode.Space) && spacebarEnabled)
        {
            // This code will run when the spacebar is pressed.
            ButtonPressDebug("Spacebar pressed!");

            CheckAndExecuteWave();
        }

        // Check if any key is pressed (For game end scenario)
        if (isGameOver && Input.anyKey)
        {
            ButtonPressDebug("Any key is pressed!");

            // Exit to main menu if any key is pressed on game over
            uiManager.GetComponent<EscapeMenuController>().ExitToMainMenu();
        }
    }

    public IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(1f);

        tutorialPanel.SetActive(true);
        // Disable space key press until the tutorial is over
        spacebarEnabled = false;
        // Muffle sound using a low pass filter
        audioMixer.SetFloat("bgmLowpass", lowpassCutoff);
    }

    public void EndTutorial()
    {
        tutorialPanel.SetActive(false);
        // Re-enable spacebar
        spacebarEnabled = true;
        interactionManager.EnableInteractions();
        // Remove muffle sound of low pass filter
        audioMixer.SetFloat("bgmLowpass", lowpassCutoffDefault);
    }

    // Below two functions control what happens when escape button is pressed
    public void OpenEscapeMenu()
    {
        PauseGame();
        interactionManager.DisableInteractions();

        escapeMenuPanel.SetActive(true);
        // Handle panels inside the escape panel
        escapeButtonPanel.SetActive(true);
        settingsPanel.SetActive(false);

        isEscaped = true;
        spacebarEnabled = false;
        // Muffle sound using a low pass filter
        audioMixer.SetFloat("bgmLowpass", lowpassCutoff);

        // Play menu open sound
        interfaceAudioManager.PlayClip(escapeMenuOpenSound);
    }

    public void CloseEscapeMenu()
    {
        // Limit certain functionality if escape menu is opened while the tutorialPanel is active
        if (!tutorialPanel.activeSelf)
        {
            interactionManager.EnableInteractions();

            spacebarEnabled = true;

            // Remove muffle sound of low pass filter
            audioMixer.SetFloat("bgmLowpass", lowpassCutoffDefault);
        }

        ResumeGame();

        // Save settings if menu is closed from the settings menu
        if (settingsPanel.activeSelf)
        {
            settingsReaderWriter.SaveSettings();
        }

        escapeMenuPanel.SetActive(false);

        isEscaped = false;

        // Play menu close sound
        interfaceAudioManager.PlayClip(escapeMenuCloseSound);
    }

    // This function is created in order for any outside class to access this class and start, pause or resume a wave
    public void CheckAndExecuteWave()
    {
        if (!enemySpawner.GetComponent<EnemySpawner>().waveOngoing) // A new wave hasn't started yet
        {
            StartWave();
        }
        else // Wave is ongoing
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void StartWave()
    {
        // Start wave
        WaveController waveController = waveManager.GetComponent<WaveController>();
        waveController.StartNewWave();

        // Change play button to pause button
        uiManager.GetComponent<PlayButtonController>().StartWave();

        isPaused = false;

        GameStateDebug("Starting new wave");
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // This will pause the game
        isPaused = true;

        // Change pause button to play button
        uiManager.GetComponent<PlayButtonController>().PauseWave();

        GameStateDebug("Game paused");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // This will resume the game
        isPaused = false;

        // Change play button to pause button if wave is ongoing
        if (enemySpawner.GetComponent<EnemySpawner>().waveOngoing)
        {
            uiManager.GetComponent<PlayButtonController>().ResumeWave();
        }

        GameStateDebug("Game resumed");
    }

    public void GameOver()
    {
        overlayPanel.SetActive(true);
        endGameText.text = "Game Over!";
        PauseGame();
        interactionManager.DisableInteractions();
        // Play audio
        interfaceAudioManager.PlayClip(gameOver, 0.3f);
        // Lower music volume
        StartCoroutine(musicManager.LowerMusic());

        isGameOver = true;
        escapeEnabled = false;
        spacebarEnabled = false;
        GameStateDebug("Game over");
    }

    public void Victory()
    {
        overlayPanel.SetActive(true);
        endGameText.text = "Victory!";
        PauseGame();
        interactionManager.DisableInteractions();
        // Play audio
        interfaceAudioManager.PlayClip(victory, 0.3f);
        // Lower music volume
        StartCoroutine(musicManager.LowerMusic());

        isGameOver = true;
        escapeEnabled = false;
        spacebarEnabled = false;
        GameStateDebug("Victory");
    }

    //--Debugs
    [Header("Debugs")]
    [SerializeField] private bool buttonPressDebug;
    [SerializeField] private bool gameStateDebug;

    private void ButtonPressDebug(string message)
    {
        if (buttonPressDebug)
        {
            Debug.Log(message);
        }
    }

    private void GameStateDebug(string message)
    {
        if (gameStateDebug)
        {
            Debug.Log(message);
        }
    }
}

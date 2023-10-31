using TMPro;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip victory;

    [Header("Game Objects")]
    [SerializeField] private GameObject overlayPanel;
    [SerializeField] private GameObject escapeMenuPanel;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject enemySpawner;

    [Header("Game Managers")]
    [SerializeField] private GameObject waveManager;
    [SerializeField] private GameObject notificationManager;
    [SerializeField] private GameInteractivity interactionManager;
    [SerializeField] private GameObject uiManager;

    [Header("Components")]
    private AudioSource audioSource;
    private InterfaceAudioHandler interfaceAudioManager; // Persistent audio manager

    [Header("Variables")]
    private bool isPaused = true;
    private bool isEscaped = false; // Turns true when escape menu is open
    //private bool gameStarted = false; // This should only be false before starting the first wave

    private void Start()
    {
        // Disable overlay panel
        overlayPanel.SetActive(false);

        // Disable escape menu
        escapeMenuPanel.SetActive(false);

        // Assign audio source
        audioSource = GetComponent<AudioSource>();

        // Find audio manager
        interfaceAudioManager = GameObject.Find("PersistentAudioManager").GetComponent<InterfaceAudioHandler>();
    }

    private void Update()
    {
        // Escape key check
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isEscaped) // Already in escape menu. Resume game
            {
                CloseEscapeMenu();
                // Play menu close sound
                interfaceAudioManager.PlayEscapeMenuCloseSound();
            }
            else // Escape
            {
                OpenEscapeMenu();
                // Play menu open sound
                interfaceAudioManager.PlayEscapeMenuOpenSound();
            }
        }

        // Check if the spacebar key is pressed and the escape menu is NOT open
        if (Input.GetKeyDown(KeyCode.Space) && !isEscaped)
        {
            // This code will run when the spacebar is pressed.
            ButtonPressDebug("Spacebar pressed!");

            CheckAndExecuteWave();
        }
    }

    // Below two functions control what happens when escape button is pressed
    public void OpenEscapeMenu()
    {
        PauseGame();
        interactionManager.DisableInteractions();
        escapeMenuPanel.SetActive(true);
        isEscaped = true;
    }

    public void CloseEscapeMenu()
    {
        ResumeGame();
        interactionManager.EnableInteractions();
        escapeMenuPanel.SetActive(false);
        isEscaped = false;
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

        // Update notification text
        NotificationController notificationController = notificationManager.GetComponent<NotificationController>();
        notificationController.ClearText();
        notificationController.DisableAnimations();

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

        // Change play button to pause button
        uiManager.GetComponent<PlayButtonController>().ResumeWave();

        GameStateDebug("Game resumed");
    }

    public void GameOver()
    {
        overlayPanel.SetActive(true);
        endGameText.text = "Game Over!";
        PauseGame();
        interactionManager.DisableInteractions();
        // Play audio
        audioSource.clip = gameOver;
        audioSource.Play();

        GameStateDebug("Game over");
    }

    public void Victory()
    {
        overlayPanel.SetActive(true);
        endGameText.text = "Victory!";
        PauseGame();
        interactionManager.DisableInteractions();
        // Play audio
        audioSource.clip = victory;
        audioSource.Play();

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

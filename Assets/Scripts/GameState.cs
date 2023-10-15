using TMPro;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip victory;

    [Header("Game Objects")]
    [SerializeField] private GameObject overlayPanel;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject waveManager;
    [SerializeField] private GameObject notificationManager;
    [SerializeField] private GameInteractivity interactionManager;

    [Header("Components")]
    private AudioSource audioSource;

    [Header("Variables")]
    private bool isPaused = false;

    private void Start()
    {
        // Disable overlay panel
        overlayPanel.SetActive(false);

        // Assign audio source
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Check if the spacebar key is pressed.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // This code will run when the spacebar is pressed.
            ButtonPressDebug("Spacebar pressed!");

            // Start wave
            WaveController waveController = waveManager.GetComponent<WaveController>();
            waveController.StartNewWave();

            // Update notification text
            NotificationController notificationController = notificationManager.GetComponent<NotificationController>();
            notificationController.ClearText();
            notificationController.DisableAnimations();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // This will pause the game
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // This will pause the game
        isPaused = false;
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
    }

    //--Debugs
    [Header("Debugs")]
    [SerializeField] private bool buttonPressDebug;

    private void ButtonPressDebug(string message)
    {
        if (buttonPressDebug)
        {
            Debug.Log(message);
        }
    }
}

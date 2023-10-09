using System;
using TMPro;
using UnityEngine;

public class GameState : MonoBehaviour
{
    //--Variables
    private bool isPaused = false;
    //--Game objects
    public GameObject overlayPanel;
    public TextMeshProUGUI endGameText;
    public GameObject waveManager;
    public GameObject notificationManager;
    [SerializeField] private GameInteractivity interactionManager;

    // Start is called before the first frame update
    void Start()
    {
        // Disable overlay panel
        overlayPanel.SetActive(false);
        // Subscribe to the event from Pathfinder.cs
        Pathfinder.GameOver += GameOver;
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

    // This triggers when the Pathfinder.cs script sends an event for the base HP reaching 0
    private void GameOver()
    {
        overlayPanel.SetActive(true);
        endGameText.text = "Game Over!";
        PauseGame();
        interactionManager.DisableInteractions();
    }

    private void PauseGame()
    {
        Time.timeScale = 0; // This will pause the game
        isPaused = true;
    }

    public void Victory()
    {
        overlayPanel.SetActive(true);
        endGameText.text = "Victory!";
        PauseGame();
        interactionManager.DisableInteractions();
    }

    //--Debugs

    public bool buttonPressDebug;

    void ButtonPressDebug(string message)
    {
        if (buttonPressDebug)
        {
            Debug.Log(message);
        }
    }
}

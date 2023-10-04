using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    //--Variables
    private bool isPaused = false;
    //--Game objects
    public GameObject overlayPanel;
    public GameObject waveManager;

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
            Debug.Log("Spacebar pressed!");

            // Start wave
            WaveController waveController = waveManager.GetComponent<WaveController>();
            waveController.StartNewWave();
        }
    }

    // This triggers when the Pathfinder.cs script sends an event for the base HP reaching 0
    private void GameOver()
    {
        overlayPanel.SetActive(true);
        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0; // This will pause the game
        isPaused = true;
    }
}

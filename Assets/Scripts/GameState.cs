using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    //--Variables
    private bool isPaused = false;
    //--Game objects
    public GameObject overlayPanel;

    // Start is called before the first frame update
    void Start()
    {
        // Disable overlay panel
        overlayPanel.SetActive(false);
        // Subscribe to the event from Pathfinder.cs
        Pathfinder.GameOver += GameOver;
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

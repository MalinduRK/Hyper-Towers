using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class EscapeMenuController : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Game Managers")]
    [SerializeField] private GameObject stateManager;

    [Header("Game Objects")]
    [SerializeField] private GameObject escapeButtonPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Components")]
    private GameState gameState;

    [Header("Variables")]
    private float lowpassCutoffDefault = 22000f; // Default cutoff frequency for the music lowpass filter

    private void Start()
    {
        // Assign GameState component
        gameState = stateManager.GetComponent<GameState>();
    }

    public void Resume()
    {
        gameState.CloseEscapeMenu();
    }

    public void Restart()
    {
        // Get the name of the currently active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Resume game
        gameState.ResumeGame();

        // Reload the current scene
        SceneManager.LoadScene(currentSceneName);

        // Remove muffle sound of low pass filter
        audioMixer.SetFloat("bgmLowpass", lowpassCutoffDefault);
    }

    public void OpenSettings()
    {
        escapeButtonPanel.SetActive(false);
        // Open in game settings menu
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        escapeButtonPanel.SetActive(true);
        // Close in game settings menu
        settingsPanel.SetActive(false);
    }

    public void ExitToMainMenu()
    {
        // Load loading screen
        SceneManager.LoadScene("LoadingScreen");

        // Load the new scene asynchronously, with the loading screen
        StartCoroutine(LoadSceneAsync("MainMenu"));

        // Set timescale to 1 (since timescale is persistent through scenes)
        Time.timeScale = 1;

        // Remove muffle sound of low pass filter
        audioMixer.SetFloat("bgmLowpass", lowpassCutoffDefault);
    }

    public void QuitToDesktop()
    {
        // Save progress code here
        //

        Debug.Log("Quitting Game");
        // Check if the application is running in the Unity Editor, as you might not want to quit during development.
        if (Application.isEditor)
        {
            // Handle quitting in the editor (e.g., stop play mode).
            //UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            // Quit the standalone player or a deployed build.
            Application.Quit();
        }
    }

    // Use this function to load scene with the loading screen
    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Load the target scene in the background.
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        // Simulate loading progress (replace this with your actual loading logic).
        while (!loadOperation.isDone)
        {
            // Update progress bar or loading screen.

            // If the load operation is almost complete, allow scene activation.
            if (loadOperation.progress >= 0.9f)
            {
                Debug.Log($"Loaded {sceneName}");
            }

            yield return null;
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevelSelectionMenu()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("CreditsScreen");
    }

    public void LoadLevel0()
    {
        // Load loading screen
        SceneManager.LoadScene("LoadingScreen");

        // Load the new scene asynchronously, with the loading screen
        StartCoroutine(LoadSceneAsync("Level0"));
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

    public void QuitGame()
    {
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
}

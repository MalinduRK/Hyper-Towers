using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{
    public void LoadLevelSelectionMenu()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Load the new scene asynchronously
        SceneManager.LoadSceneAsync("LevelSelection");
    }

    public void LoadMainMenu()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Load the new scene asynchronously
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void LoadLevel0()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Load the new scene asynchronously
        SceneManager.LoadSceneAsync("Level0");
    }

    public void QuitGame()
    {
        // Check if the application is running in the Unity Editor, as you might not want to quit during development.
        if (Application.isEditor)
        {
            // Handle quitting in the editor (e.g., stop play mode).
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            // Quit the standalone player or a deployed build.
            Application.Quit();
        }
    }
}

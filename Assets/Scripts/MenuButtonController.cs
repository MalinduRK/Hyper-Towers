using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{
    public void LoadMainMenu()
    {
        // Load the new scene asynchronously
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSettings()
    {
        // Load the new scene asynchronously
        SceneManager.LoadScene("SettingsMenu");
    }

    public void LoadLevelSelectionMenu()
    {
        // Load the new scene asynchronously
        SceneManager.LoadScene("LevelSelection");
    }

    public void LoadLevel0()
    {
        // Load the new scene asynchronously
        SceneManager.LoadScene("Level0");
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

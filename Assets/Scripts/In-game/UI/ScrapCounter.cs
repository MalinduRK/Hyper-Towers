using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// All scrap related functions are done through this class
public class ScrapCounter : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private TextMeshProUGUI scrapCountText; // Scrap text
    [SerializeField] private GameObject dataManager;

    [Header("Variables")]
    private int scrapCount; // Universal scrap count

    private void Start()
    {
        // Access the name of the current level (scene)
        Scene currentScene = SceneManager.GetActiveScene(); // Get the currently active scene
        string sceneName = currentScene.name;

        // Extract the number from the scene name using regular expressions.
        Match match = Regex.Match(sceneName, @"\d+");

        int currentLevel; // Pre-initialized to avoid issues

        if (match.Success)
        {
            // Convert the matched string to an integer.
            currentLevel = int.Parse(match.Value);

            // Now you have the isolated scene number in the 'currentLevel' variable.
            Debug.Log("Level: " + currentLevel);
        }
        else
        {
            // Handle the case where a number was not found in the scene name.
            Debug.LogError("Level number not found in scene name");

            return;
        }

        // Assign DataReader.cs
        DataReader dataReader = dataManager.GetComponent<DataReader>();

        // Access level data for the current level
        LevelData levelData = dataReader.ReadLevelData(currentLevel);

        // Assign the starting scrap value to the universal scrap count
        scrapCount = levelData.starting_scrap;
        // Update UI
        scrapCountText.text = scrapCount.ToString();
    }

    // Add scrap to the scrap pool
    public void AddScrap(int scrapValue)
    {
        scrapCount += scrapValue; // Add the scrap amount to the scrap pool
        UpdateScrap();
    }

    public void UseScrap(int scrapValue)
    {
        scrapCount -= scrapValue;
        UpdateScrap();
    }

    // Return current scrap value
    public int GetScrap()
    {
        return scrapCount;
    }

    // Update the displayed scrap value in the UI
    public void UpdateScrap()
    {
        scrapCountText.text = scrapCount.ToString();
    }
}

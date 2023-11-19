using System;
using System.IO;
using UnityEngine;

// Data class to store the settings json structure
[System.Serializable]
public class LevelStatusData
{
    public bool tutorialCompleted;
}

public class LevelStatus : MonoBehaviour
{
    [Header("Variables")]
    private string filePath;

    private LevelStatusData levelStatusData;
    private void Start()
    {
        filePath = Application.persistentDataPath + "/level_status.json";

        // Load data on start
        LoadLevelStatus();
    }

    public void CompleteTutorial()
    {
        string json = File.ReadAllText(filePath);
        levelStatusData = JsonUtility.FromJson<LevelStatusData>(json);

        // String which holds the updated json data later in the code
        string updatedJson = JsonUtility.ToJson(levelStatusData);

        // Mark tutorial completion as true
        levelStatusData.tutorialCompleted = true;

        // Write new json data to file
        try
        {
            // Attempt to write the JSON string to the file
            File.WriteAllText(filePath, updatedJson);

            // If the write operation is successful, display success message
            Debug.Log("Marked tutorial completion");
        }
        catch (Exception e)
        {
            // If there was an error, catch the exception and display error message
            Debug.LogError("Failed to mark tutorial completion: " + e.Message);
        }
    }

    // Run this at the start of the first scene
    public void LoadLevelStatus()
    {
        if (File.Exists(filePath)) // Load data
        {
            string json = File.ReadAllText(filePath);
            levelStatusData = JsonUtility.FromJson<LevelStatusData>(json);

            Debug.Log($"Level status data loaded from file {filePath}");
        }
        else // Create new data file
        {
            CreateFile();
        }
    }

    // Run this whenever needed after the first scene
    public LevelStatusData ReadLevelStatus()
    {
        if (File.Exists(filePath)) // Load data
        {
            string json = File.ReadAllText(filePath);
            levelStatusData = JsonUtility.FromJson<LevelStatusData>(json);
        }

        return levelStatusData;
    }

    private void CreateFile()
    {
        // Set new data into json format
        levelStatusData = new LevelStatusData
        {
            tutorialCompleted = false
        };

        string updatedJson = JsonUtility.ToJson(levelStatusData);

        // Write new json data to file
        try
        {
            // Attempt to write the JSON string to the file
            File.WriteAllText(filePath, updatedJson);

            // If the write operation is successful, display success message
            Debug.Log($"Created new level_status data file at: {filePath}");
        }
        catch (Exception e)
        {
            // If there was an error, catch the exception and display error message
            Debug.LogError($"Failed to create level_status data file at {filePath}: " + e.Message);
        }
    }
}

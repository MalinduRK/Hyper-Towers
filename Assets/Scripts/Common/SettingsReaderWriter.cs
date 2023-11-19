using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Data class to store the settings json structure
[System.Serializable]
public class SettingsData
{
    public int resolutionWidth;
    public int resolutionHeight;
    public int resolutionRefreshRate;
    public int windowMode;
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;
}

public class SettingsReaderWriter : MonoBehaviour
{
    [Header("Assets")]
    public AudioMixer audioMixer;

    [Header("Game Objects")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown windowModeDropdown;
    [SerializeField] private TextMeshProUGUI successMessageText;

    [Header("Variables")]
    private string filePath; // Data can be read through a referred text asset but cannot be written into. Therefore the file path is necessary to write data

    private SettingsData settingsData;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/settings.json";
        //Debug.Log(filePath);
    }

    public void SaveSettings()
    {
        // Load existing settings
        filePath = Application.persistentDataPath + "/settings.json";
        string json = File.ReadAllText(filePath);
        settingsData = JsonUtility.FromJson<SettingsData>(json);

        // String which holds the updated json data later in the code
        string updatedJson = JsonUtility.ToJson(settingsData);

        // Save the below settings only if the current scene is SettingsMenu
        if (SceneManager.GetActiveScene().name == "SettingsMenu")
        {
            // Get resolution string from the dropdown and extract width and height values
            string resolutionString = resolutionDropdown.options[resolutionDropdown.value].text; // Get current value of the dropdown
            //Debug.Log(resolutionString);

            string[] parts = resolutionString.Split('x'); // Split the string using 'x' as the delimiter
                                                          // Initialize variables and assign default values
            int resolutionWidth = 1280;
            int resolutionHeight = 720;

            if (parts.Length == 2)
            {
                if (int.TryParse(parts[0].Trim(), out resolutionWidth))
                {
                    if (int.TryParse(parts[1].Trim(), out resolutionHeight))
                    {
                        // Now you have resolutionWidth and resolutionHeight as integers
                        Debug.Log("Width: " + resolutionWidth);
                        Debug.Log("Height: " + resolutionHeight);
                    }
                    else
                    {
                        Debug.LogError("Failed to parse resolution height.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to parse resolution width.");
                }
            }
            else
            {
                Debug.LogError("Invalid format for resolution string.");
            }

            // Get new settings
            int resolutionRefreshRate = Mathf.RoundToInt(float.Parse(Screen.currentResolution.refreshRateRatio.ToString()));
            int windowModeIndex = windowModeDropdown.value;

            // Update specific fields
            settingsData.resolutionWidth = resolutionWidth;
            settingsData.resolutionHeight = resolutionHeight;
            settingsData.resolutionRefreshRate = resolutionRefreshRate;
            settingsData.windowMode = windowModeIndex;

            // Convert updated settings back to JSON
            updatedJson = JsonUtility.ToJson(settingsData);
        }

        // Set new settings into json format
        /*settingsData = new SettingsData
        {
            resolutionWidth = resolutionWidth,
            resolutionHeight = resolutionHeight,
            resolutionRefreshRate = resolutionRefreshRate,
            windowMode = windowModeIndex,
            masterVolume = currentMasterVolume,
            bgmVolume = currentBGMVolume,
            sfxVolume = currentSFXVolume
        };*/

        audioMixer.GetFloat("masterVolume", out float currentMasterVolume);
        audioMixer.GetFloat("bgmVolume", out float currentBGMVolume);
        audioMixer.GetFloat("sfxVolume", out float currentSFXVolume);

        // Update specific fields
        settingsData.masterVolume = currentMasterVolume;
        settingsData.bgmVolume = currentBGMVolume;
        settingsData.sfxVolume = currentSFXVolume;

        updatedJson = JsonUtility.ToJson(settingsData);

        // Write new json data to file
        try
        {
            // Attempt to write the JSON string to the file
            File.WriteAllText(filePath, updatedJson);

            // If the write operation is successful, display success message
            Debug.Log("Successfully applied settings");
            if (successMessageText != null) // This message doesn't display in-game
            {
                successMessageText.text = "Settings applied";
            }
            // Start a coroutine to clear the text after 2 seconds
            StartCoroutine(ClearTextAfterDelayCoroutine());
        }
        catch (Exception e)
        {
            // If there was an error, catch the exception and display error message
            Debug.LogError("Failed to apply settings: " + e.Message);
            if (successMessageText != null) // This message doesn't display in-game
            {
                successMessageText.text = "Failed to apply settings";
            }
            // Start a coroutine to clear the text after 2 seconds
            StartCoroutine(ClearTextAfterDelayCoroutine());
        }
    }

    public void LoadSettings()
    {
        filePath = Application.persistentDataPath + "/settings.json";

        //Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            settingsData = JsonUtility.FromJson<SettingsData>(json);

            // Set resolution and window mode
            Screen.SetResolution(settingsData.resolutionWidth, settingsData.resolutionHeight, Screen.fullScreenMode);
            switch (settingsData.windowMode)
            {
                case 0: // Fullscreen
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;

                case 1: // Borderless Windowed
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                case 2: // Windowed
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
            }
            // Set volumes in the audio mixer
            audioMixer.SetFloat("masterVolume", settingsData.masterVolume);
            audioMixer.SetFloat("bgmVolume", settingsData.bgmVolume);
            audioMixer.SetFloat("sfxVolume", settingsData.sfxVolume);

            Debug.Log($"Settings loaded from file {filePath}");
        }
        else // Create file and write default data
        {
            // Get current screen resolution
            Resolution resolution = Screen.currentResolution;

            int resolutionWidth = resolution.width;
            int resolutionHeight = resolution.height;
            int resolutionRefreshRate = Mathf.RoundToInt(float.Parse(resolution.refreshRateRatio.ToString()));

            // Set default window mode
            int windowModeIndex = 1; // TODO: Confirm this

            // Get current window mode
            if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
            {
                windowModeIndex = 0;
            }
            else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            {
                windowModeIndex = 1;
            }
            else
            {
                windowModeIndex = 2;
            }

            // Set default volumes (0 db is the max volume and means the opposite of muted)
            int currentMasterVolume = 0;
            int currentBGMVolume = 0;
            int currentSFXVolume = 0;

            // Set new settings into json format
            settingsData = new SettingsData
            {
                resolutionWidth = resolutionWidth,
                resolutionHeight = resolutionHeight,
                resolutionRefreshRate = resolutionRefreshRate,
                windowMode = windowModeIndex,
                masterVolume = currentMasterVolume,
                bgmVolume = currentBGMVolume,
                sfxVolume = currentSFXVolume
            };

            string json = JsonUtility.ToJson(settingsData);

            // Write new json data to file
            try
            {
                // Attempt to write the JSON string to the file
                File.WriteAllText(filePath, json);

                // If the write operation is successful, display success message
                Debug.Log("Successfully applied settings");
                if (successMessageText != null) // This message doesn't display in-game
                {
                    successMessageText.text = "Settings applied";
                }
                // Start a coroutine to clear the text after 2 seconds
                StartCoroutine(ClearTextAfterDelayCoroutine());
            }
            catch (Exception e)
            {
                // If there was an error, catch the exception and display error message
                Debug.LogError("Failed to apply settings: " + e.Message);
                if (successMessageText != null) // This message doesn't display in-game
                {
                    successMessageText.text = "Failed to apply settings";
                }
                // Start a coroutine to clear the text after 2 seconds
                StartCoroutine(ClearTextAfterDelayCoroutine());
            }
        }
    }

    // This function will only return the settings values without applying them
    public SettingsData ReadSettings()
    {
        filePath = Application.persistentDataPath + "/settings.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            settingsData = JsonUtility.FromJson<SettingsData>(json);
        }

        return settingsData;
    }

    // Use this function to clear text (TODO: Replace with an animation)
    private IEnumerator ClearTextAfterDelayCoroutine()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        if (successMessageText != null)
        {
            // Clear the text
            successMessageText.text = "";
        }
    }
}

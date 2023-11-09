using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

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
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown windowModeDropdown;

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
        audioMixer.GetFloat("masterVolume", out float currentMasterVolume);
        audioMixer.GetFloat("bgmVolume", out float currentBGMVolume);
        audioMixer.GetFloat("sfxVolume", out float currentSFXVolume);

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
        File.WriteAllText(filePath, json);

        Debug.Log("Settings applied");
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
}

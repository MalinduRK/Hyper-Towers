using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsReaderWriter : MonoBehaviour
{
    // Data class to store the settings json structure
    [System.Serializable]
    private class SettingsData
    {
        public int resolutionWidth;
        public int resolutionHeight;
        public int windowMode;
        public float masterVolume;
        public float bgmVolume;
        public float sfxVolume;
    }

    [Header("Assets")]
    public AudioMixer audioMixer;

    [Header("Game Objects")]
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
        // Get new settings
        int resolutionWidth = Screen.currentResolution.width;
        int resolutionHeight = Screen.currentResolution.height;
        int windowModeIndex = windowModeDropdown.value;
        audioMixer.GetFloat("masterVolume", out float currentMasterVolume);
        audioMixer.GetFloat("bgmVolume", out float currentBGMVolume);
        audioMixer.GetFloat("sfxVolume", out float currentSFXVolume);

        // Set new settings into json format
        settingsData = new SettingsData
        {
            resolutionWidth = resolutionWidth,
            resolutionHeight = resolutionHeight,
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
            Screen.SetResolution(settingsData.resolutionWidth, settingsData.resolutionHeight, Screen.fullScreen);
            switch (settingsData.windowMode)
            {
                case 0: // Fullscreen
                    Screen.fullScreen = true;
                    break;

                case 1: // Windowed
                    Screen.fullScreen = false;
                    break;
            }
            // Set volumes in the audio mixer
            audioMixer.SetFloat("masterVolume", settingsData.masterVolume);
            audioMixer.SetFloat("bgmVolume", settingsData.bgmVolume);
            audioMixer.SetFloat("sfxVolume", settingsData.sfxVolume);

            Debug.Log($"Settings loaded from file {filePath}");
        }
    }
}

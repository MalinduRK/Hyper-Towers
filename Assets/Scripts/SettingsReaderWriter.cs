using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsReaderWriter : MonoBehaviour
{
    // Data class to store the settings json structure
    [System.Serializable]
    private class SettingsData
    {
        public int windowMode;
        public float masterVolume;
        public float bgmVolume;
        public float sfxVolume;
    }

    public AudioMixer audioMixer;

    private string filePath; // Data can be read through a referred text asset but cannot be written into. Therefore the file path is necessary to write data

    private SettingsData settingsData;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/settings.json";
        //Debug.Log(filePath);
    }

    public void SaveSettings()
    {
        audioMixer.GetFloat("masterVolume", out float currentMasterVolume);
        audioMixer.GetFloat("bgmVolume", out float currentBGMVolume);
        audioMixer.GetFloat("sfxVolume", out float currentSFXVolume);

        settingsData = new SettingsData
        {
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

            // Set volumes in the audio mixer
            audioMixer.SetFloat("masterVolume", settingsData.masterVolume);
            audioMixer.SetFloat("bgmVolume", settingsData.bgmVolume);
            audioMixer.SetFloat("sfxVolume", settingsData.sfxVolume);

            Debug.Log($"Settings loaded from file {filePath}");
        }
    }
}

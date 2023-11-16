using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Assets")]
    public AudioMixer audioMixer;

    [Header("Game Objects")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown windowModeDropdown;

    [Header("Components")]
    private SettingsData settingsData;

    [Header("Variables")]
    private Resolution[] resolutions; // Variable to store all resolutions related to the current device
    private float volumeMinValue; // Volume will drop to -80 when it reaches the min value of the slider

    private void Start()
    {
        // Assign settingsData
        settingsData = GetComponent<SettingsReaderWriter>().ReadSettings();

        // If this is not the SettingsMenu scene, the following functions should not run
        if (SceneManager.GetActiveScene().name == "SettingsMenu")
        {
            // Get all resolutions matching the current device screen
            resolutions = Screen.resolutions;

            // Clear the options of the resolution dropdown at start
            resolutionDropdown.ClearOptions();

            // Create list to store all viable resolutions as a string
            List<string> resolutionOptions = new List<string>();

            // Variable to store current screen resolution
            int currentResolutionIndex = 0;

            // Add all resolutions to the list
            for (int i = 0; i < resolutions.Length; i++)
            {
                // Take screen refresh rate and round off to nearest int
                int refreshRate = Mathf.RoundToInt(float.Parse(resolutions[i].refreshRateRatio.ToString()));

                // If the previous resolution has the same value, don't add this one to the list
                if (i != 0)
                {
                    if (resolutions[i].width == resolutions[i - 1].width && resolutions[i].height == resolutions[i - 1].height)
                    {
                        return;
                    }
                }

                string option = $"{resolutions[i].width} x {resolutions[i].height}";
                resolutionOptions.Add(option);

                // Set the current resolution from the list of resolutions
                if (resolutions[i].width == settingsData.resolutionWidth && resolutions[i].height == settingsData.resolutionHeight)
                {
                    currentResolutionIndex = i;
                }
            }

            // Add new options to resolution dropdown
            resolutionDropdown.AddOptions(resolutionOptions);
            // Display current resolution on the dropdown menu
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();


            // Set current window mode in the dropdown on start
            int currentWindowModeIndex = settingsData.windowMode;

            // Set and refresh dropdown
            windowModeDropdown.value = currentWindowModeIndex;
            windowModeDropdown.RefreshShownValue();

            SettingsDebug($"Resolution: {resolutionDropdown.value} | Window mode: {windowModeDropdown.value}");
        }

        // Variables to store current volumes
        float currentMasterVolume;
        float currentBGMVolume;
        float currentSFXVolume;

        // Set the volume sliders to the current volume on load
        currentMasterVolume = settingsData.masterVolume;
        currentBGMVolume = settingsData.bgmVolume;
        currentSFXVolume = settingsData.sfxVolume;

        // Get the min value of the slider
        volumeMinValue = masterVolumeSlider.GetComponent<Slider>().minValue;

        // Make sure all the volumes stay at -80 if the sliders are all the way down

        // Master
        if (currentMasterVolume > volumeMinValue)
        {
            masterVolumeSlider.GetComponent<Slider>().value = currentMasterVolume;
        }
        else
        {
            masterVolumeSlider.GetComponent<Slider>().value = volumeMinValue;
        }

        // BGM
        if (currentBGMVolume > volumeMinValue)
        {
            bgmVolumeSlider.GetComponent<Slider>().value = currentBGMVolume;
        }
        else
        {
            bgmVolumeSlider.GetComponent<Slider>().value = volumeMinValue;
        }

        // SFX
        if (currentSFXVolume > volumeMinValue)
        {
            sfxVolumeSlider.GetComponent<Slider>().value = currentSFXVolume;
        }
        else
        {
            sfxVolumeSlider.GetComponent<Slider>().value = volumeMinValue;
        }

        SettingsDebug($"Master volume: {currentMasterVolume} | BGM volume: {currentBGMVolume} | SFX volume: {currentSFXVolume}");
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    public void SetWindowMode(int windowModeIndex)
    {
        switch (windowModeIndex)
        {
            case 0: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1: // Borderless window
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 2: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    // Below are the functions to set volumes using sliders

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);

        // Mute if the slider is all the way down
        if (volume == volumeMinValue)
        {
            audioMixer.SetFloat("masterVolume", -80);
        }
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("bgmVolume", volume);

        // Mute if the slider is all the way down
        if (volume == volumeMinValue)
        {
            audioMixer.SetFloat("bgmVolume", -80);
        }
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);

        // Mute if the slider is all the way down
        if (volume == volumeMinValue)
        {
            audioMixer.SetFloat("sfxVolume", -80);
        }
    }


    [Header("Debug")]

    [SerializeField] private bool settingsDebug;

    private void SettingsDebug(string message)
    {
        if (settingsDebug)
        {
            Debug.Log(message);
        }
    }
}

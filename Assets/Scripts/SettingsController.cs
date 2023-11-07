using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] GameObject masterVolumeSlider;
    [SerializeField] GameObject bgmVolumeSlider;
    [SerializeField] GameObject sfxVolumeSlider;

    public AudioMixer audioMixer;

    private float volumeMinValue; // Volume will drop to -80 when it reaches the min value of the slider

    private void Start()
    {
        // Variables to store current volumes
        float currentMasterVolume;
        float currentBGMVolume;
        float currentSFXVolume;

        // Set the volume sliders to the current volume of the audio mixer on load
        audioMixer.GetFloat("masterVolume", out currentMasterVolume);
        audioMixer.GetFloat("bgmVolume", out currentBGMVolume);
        audioMixer.GetFloat("sfxVolume", out currentSFXVolume);

        // Get the min value of the slider
        volumeMinValue = masterVolumeSlider.GetComponent<Slider>().minValue;

        // Make sure all the volumes stay at -80 if the sliders are all the way down
        SettingsDebug($"Master volume: {currentMasterVolume} | BGM volume: {currentBGMVolume} | SFX volume: {currentSFXVolume}");

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

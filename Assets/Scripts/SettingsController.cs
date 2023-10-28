using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] GameObject masterVolumeSlider;

    public AudioMixer audioMixer;

    private float volumeMinValue; // Volume will drop to -80 when it reaches the min value of the slider

    private void Start()
    {
        float currentMasterVolume;
        // Set the volume sliders to the current volume of the audio mixer on load
        audioMixer.GetFloat("masterVolume", out currentMasterVolume);

        // Get the min value of the slider
        volumeMinValue = masterVolumeSlider.GetComponent<Slider>().minValue;

        // Make sure the master volume stays at -80 if the slider is all the way down
        if (currentMasterVolume > volumeMinValue)
        {
            masterVolumeSlider.GetComponent<Slider>().value = currentMasterVolume;
        }
        else
        {
            masterVolumeSlider.GetComponent<Slider>().value = volumeMinValue;
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);

        // Mute if the slider is all the way down
        if (volume == volumeMinValue)
        {
            audioMixer.SetFloat("masterVolume", -80);
        }
    }
}

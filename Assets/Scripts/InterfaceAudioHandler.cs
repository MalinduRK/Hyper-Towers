using UnityEngine;
using UnityEngine.Rendering;

public class InterfaceAudioHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource interfaceAudioSource;

    [Header("Variables")]
    [SerializeField] private float defaultVolume; // Set this to the default volume as required by most audio clips

    public void PlayClip(AudioClip clip)
    {
        // Set the volume to the default volume
        interfaceAudioSource.volume = defaultVolume;

        interfaceAudioSource.clip = clip;
        interfaceAudioSource.Play();
    }

    public void PlayClip(AudioClip clip, float volume)
    {
        // Set the volume to specified volume
        interfaceAudioSource.volume = volume;

        interfaceAudioSource.clip = clip;
        interfaceAudioSource.Play();
    }
}

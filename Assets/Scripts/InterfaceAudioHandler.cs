using UnityEngine;

public class InterfaceAudioHandler : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private AudioClip menuButtonClickSound;
    [SerializeField] private AudioClip menuBackButtonClickSound;
    [SerializeField] private AudioClip menuButtonHoverSound;
    [SerializeField] private AudioClip escapeMenuOpenSound;
    [SerializeField] private AudioClip escapeMenuCloseSound;

    [Header("Components")]
    [SerializeField] private AudioSource interfaceAudioSource;

    public void PlayMenuClickSound()
    {
        interfaceAudioSource.clip = menuButtonClickSound;
        interfaceAudioSource.Play();
    }

    public void PlayMenuBackClickSound()
    {
        interfaceAudioSource.clip = menuBackButtonClickSound;
        interfaceAudioSource.Play();
    }

    public void PlayMenuButtonHoverSound()
    {
        interfaceAudioSource.clip = menuButtonHoverSound;
        interfaceAudioSource.Play();
    }

    public void PlayEscapeMenuOpenSound()
    {
        interfaceAudioSource.clip = escapeMenuOpenSound;
        interfaceAudioSource.Play();
    }

    public void PlayEscapeMenuCloseSound()
    {
        interfaceAudioSource.clip = escapeMenuCloseSound;
        interfaceAudioSource.Play();
    }
}

using UnityEngine;

public class ClickAudioHandler : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private AudioClip menuButtonClickSound;

    [Header("Components")]
    [SerializeField] private AudioSource sfxAudioSource;

    public void PlayMenuClickSound()
    {
        sfxAudioSource.clip = menuButtonClickSound;
        sfxAudioSource.Play();
    }
}

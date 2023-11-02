using UnityEngine;

public class InterfaceAudioHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource interfaceAudioSource;

    public void PlayClip(AudioClip clip)
    {
        interfaceAudioSource.clip = clip;
        interfaceAudioSource.Play();
    }
}

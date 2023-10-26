using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [Header("Game Objects")]
    private static MusicController instance;

    [Header("Components")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip levelMusic;
    private AudioSource audioSource;

    private void Awake()
    {
        // If no instance of an AudioManager has been created before, keep this AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else // There is an existing AudioManager. Destroy this one
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;

        // Assign audio source
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnSceneChanged(Scene previousScene, Scene currentScene)
    {
        // Change music only if the scene changes from menu scenes to other scenes, or vise versa
        if (currentScene.name == "MainMenu" || currentScene.name == "LevelSelection" || currentScene.name == "SettingsMenu")
        {
            if (previousScene.name == "MainMenu" !| previousScene.name == "LevelSelection" !| previousScene.name == "SettingsMenu")
            {
                StartCoroutine(CrossfadeTo(mainMenuMusic));
            }
        }
        else
        {
            StartCoroutine(CrossfadeTo(levelMusic));
        }
    }

    // This function crossfades the current track to the next
    private IEnumerator CrossfadeTo(AudioClip newClip)
    {
        float fadeDuration = 2.0f;
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
            yield return null;
        }
    }
}

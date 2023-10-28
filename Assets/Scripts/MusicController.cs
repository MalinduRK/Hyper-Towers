using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [Header("Game Objects")]
    private static MusicController instance;

    [Header("Components")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip levelMusic;

    [Header("Variables")]
    private string activeSceneName; // Variable to store the active scene name to use for checking previous scene, since the parameter doesn't work for some reason

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
        //musicAudioSource = gameObject.GetComponent<AudioSource>();

        activeSceneName = SceneManager.GetActiveScene().name;
    }

    private void OnSceneChanged(Scene previousScene, Scene currentScene)
    {
        // Set active scene to previous scene
        string lastSceneName = activeSceneName;

        Debug.Log($"Scene changed. Previous: {lastSceneName}, Current: {currentScene.name}");

        // Change music only if the scene changes from menu scenes to other scenes, or vise versa
        /*if (currentScene.name == "MainMenu" || currentScene.name == "LevelSelection" || currentScene.name == "SettingsMenu")
        {
            if (!(lastSceneName == "MainMenu" || lastSceneName == "LevelSelection" || lastSceneName == "SettingsMenu")) // Coming from a game scene
            {
                StartCoroutine(CrossfadeTo(mainMenuMusic));
            }
        }
        else
        {
            StartCoroutine(CrossfadeTo(levelMusic));
        }*/

        if (lastSceneName == "LoadingScreen")
        {
            if (currentScene.name == "MainMenu")
            {
                StartCoroutine(CrossfadeTo(mainMenuMusic));
            }
            else
            {
                StartCoroutine(CrossfadeTo(levelMusic));
            }
        }

        // Set current scene to active scene for use in next scene change
        activeSceneName = currentScene.name;
    }

    // This function crossfades the current track to the next
    private IEnumerator CrossfadeTo(AudioClip newClip)
    {
        float fadeDuration = 2.0f;
        float startVolume = musicAudioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            //Debug.Log($"timer = {timer}");
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        Debug.Log($"Changing music to {newClip.name}");

        musicAudioSource.clip = newClip;
        musicAudioSource.Play();

        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
            yield return null;
        }
    }
}

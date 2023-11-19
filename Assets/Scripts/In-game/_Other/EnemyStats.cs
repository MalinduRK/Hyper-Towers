using UnityEngine;
using UnityEngine.Audio;

public class EnemyStats : MonoBehaviour
{
    [Header("Assets")]
    public AudioMixer audioMixer;
    [SerializeField] private AudioClip explosionSound;

    [Header("Game Objects")]
    public GameObject explosionPrefab;
    private GameObject currentHealthBar;
    //private GameObject scrapManager; // Reference to the scrap manager

    [Header("Components")]
    private ScrapCounter scrapCounter; //  Reference to the ScrapCounter script
    private DataReader dataReader;

    [Header("Variables")]
    public EnemyData enemyData = new EnemyData();
    private float enemyMaxHP; // Max HP of enemy
    private float lengthFactor; // Factor which represents 1 unit of HP in terms of length of the health bar

    // Start is called before the first frame update
    private void Start()
    {
        // Get the name of the current GameObject (It's always in the format "EnemyX(Clone)")
        string gameObjectName = gameObject.name;

        // Remove "(Clone)" from the name
        string enemyName = gameObjectName.Replace("(Clone)", "");

        // Assign data reader
        GameObject dataReaderObject = GameObject.Find("DataManager");
        dataReader = dataReaderObject.GetComponent<DataReader>();

        // Read and assign enemy data
        enemyData = dataReader.ReadEnemyData(enemyName);

        // Store enemy max HP for future reference
        enemyMaxHP = enemyData.health;

        // Find and assign health bar
        currentHealthBar = gameObject.transform.Find("Canvas").Find("HealthBar").gameObject;

        Vector3 scale = currentHealthBar.transform.localScale;

        // Find scrap manager and assign scrapCounter
        GameObject scrapManager = GameObject.Find("ScrapManager");
        scrapCounter = scrapManager.GetComponent<ScrapCounter>();

        // Set the length factor
        lengthFactor = scale.x / enemyMaxHP;
        // Multiply any HP unit by the length factor to get the actual length of the health bar

        //Debug.Log($"Length factor: {lengthFactor}");
    }

    public void Hit(float hitDamage)
    {
        // Damage enemy
        enemyData.health -= hitDamage;

        if (enemyData.health <= 0) // Enemy is dead
        {
            Destroy(gameObject);

            // Instantiate the destroy particle system at the enemy's position
            GameObject particleSystem = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // Get the main module of the Particle System
            ParticleSystem.MainModule mainModule = explosionPrefab.GetComponent<ParticleSystem>().main;
            // Destroy the explosionPrefab after the duration of the Particle System
            Destroy(particleSystem, mainModule.duration);

            // Play explosion sound
            float randVolume = Random.Range(0.1f, 0.3f); // Add random volume
            audioMixer.GetFloat("sfxVolume", out float currentSFXVolume); // Get sfx volume
            // Convert audio mixer volume to actual volume units of the audio source (the volume sliders in settings go from 0 to -20)
            float sfxVolumePercent = 0; // Set volume to 0 if audio mixer is at -80db
            if (currentSFXVolume != -80) // If not, set volume
            {
                sfxVolumePercent = (currentSFXVolume + 20) / 20;
            }

            float volume = randVolume * sfxVolumePercent;
            
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, volume);

            // Add 1 scrap to the total scrap count
            scrapCounter.AddScrap(enemyData.scrap_value);
        }
        else
        {
            // Update enemy health bar
            Vector3 newScale = currentHealthBar.transform.localScale;
            newScale.x = enemyData.health * lengthFactor;
            currentHealthBar.transform.localScale = newScale;
        }
    }
}

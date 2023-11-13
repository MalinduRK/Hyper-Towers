using TMPro;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private AudioClip baseBreach;

    [Header("Game Objects")]
    [SerializeField] private TextMeshProUGUI baseHP; // Base HP text

    [Header("Components")]
    private AudioSource audioSource;
    public ParticleSystem damageParticleSystem;
    private ParticleSystem.EmissionModule emissionModule;

    [Header("Variables")]
    public float rotationSpeed = 45.0f; // Adjust the rotation speed as needed.
    private int baseHPValue; // Get current base HP
    float damageEmissionRate = 0; // Starting emission of the base damage particles
    float damageEmissionPerEnemy; // This is the amount of emission increase per enemy breaching the base

    private void Start()
    {
        // Assign audio source
        audioSource = GetComponent<AudioSource>();

        // Assign starting base HP
        baseHPValue = int.Parse(baseHP.text);

        // Higher the base HP, lesser the damage emission one breach does to the base
        damageEmissionPerEnemy = 100 / baseHPValue;

        // Assign emission module of the particle system
        emissionModule = damageParticleSystem.emission;

        // Set the starting emission rate
        emissionModule.rateOverTime = damageEmissionRate;
    }

    private void Update()
    {
        // Rotate the object around its Y-axis.
        //transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    public void BaseBreach()
    {
        // Reduce base HP value by 1
        baseHPValue--;
        baseHP.text = baseHPValue.ToString();
        // Update damage emission
        AdjustEmissionRate();

        // Game Over if base HP == 0
        if (baseHPValue == 0)
        {
            // Trigger game over
            GameState gameState = GameObject.Find("StateManager").GetComponent<GameState>();
            gameState.GameOver();

            return;
        }

        // Play audio
        audioSource.clip = baseBreach;
        audioSource.Play();
    }

    private void AdjustEmissionRate()
    {
        damageEmissionRate += damageEmissionPerEnemy;

        // Set the emission rate
        emissionModule.rateOverTime = damageEmissionRate;
    }
}

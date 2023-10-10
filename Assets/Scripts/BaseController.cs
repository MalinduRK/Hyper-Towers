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

    [Header("Variables")]
    public float rotationSpeed = 45.0f; // Adjust the rotation speed as needed.

    private void Start()
    {
        // Assign audio source
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Rotate the object around its Y-axis.
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    public void BaseBreach()
    {
        // Get current base HP
        int baseHPValue = int.Parse(baseHP.text);
        // Reduce base HP value by 1
        baseHPValue--;
        baseHP.text = baseHPValue.ToString();

        // Game Over if base HP == 0
        if (baseHPValue == 0)
        {
            // Trigger game over
            GameState gameState = GameObject.Find("StateManager").GetComponent<GameState>();
            gameState.GameOver();
        }

        // Play audio
        audioSource.clip = baseBreach;
        audioSource.Play();
    }
}

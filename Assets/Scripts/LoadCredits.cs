using TMPro;
using UnityEngine;

public class LoadCredits : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private TextAsset attributesText; // .txt file containing all attributes for the game

    private void Start()
    {
        TextMeshProUGUI creditsText = GetComponent<TextMeshProUGUI>();
        // Write file content into text component
        creditsText.text = attributesText.text;
    }
}

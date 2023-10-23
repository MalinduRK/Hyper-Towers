using UnityEngine;
using UnityEngine.UI;

public class PlayButtonController : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Sprite play;
    [SerializeField] private Sprite pause;

    [Header("Game Objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject stateManagerObject;

    // This function runs when the button is clicked on as well as from other functions
    public void StartWave()
    {
        // Call start wave function from the state manager
        stateManagerObject.GetComponent<GameState>().StartWave();

        // Convert play button into pause button
        playButton.GetComponent<Image>().sprite = pause;
    }

    // This function is only triggered by other functions
    public void EndWave()
    {
        // Convert pause button into play button
        playButton.GetComponent<Image>().sprite = play;
    }
}

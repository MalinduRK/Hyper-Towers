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
        // Convert play button into pause button
        playButton.GetComponent<Image>().sprite = pause;
    }

    public void PauseWave()
    {
        // Convert pause button into play button
        playButton.GetComponent<Image>().sprite = play;
    }

    public void ResumeWave()
    {
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

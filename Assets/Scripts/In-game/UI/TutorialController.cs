using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Game Obejcts")]
    [SerializeField] private GameObject stateManager;
    [SerializeField] private GameObject dataManager;
    [SerializeField] private GameObject[] tutorialPanes;

    [Header("Variables")]
    private int activePaneIndex = 0; // Store the index of the active tutorial pane
    private int totalPanels; // Total number of tutorial panes in the array

    private void Start()
    {
        // Store the total number of tutorial panes in the array
        totalPanels = tutorialPanes.Length;

        // Disable all tutorial panes
        foreach (var tutorialPane in tutorialPanes)
        {
            tutorialPane.SetActive(false);
        }

        // Set the first pane active
        tutorialPanes[activePaneIndex].SetActive(true);
    }

    // Go to next panel
    public void NextPanel()
    {
        if (activePaneIndex < totalPanels - 1)
        {
            activePaneIndex++;
            // Enable new panel
            tutorialPanes[activePaneIndex].SetActive(true);
            // Disable previous panel
            tutorialPanes[activePaneIndex - 1].SetActive(false);
        }
    }

    // Go to previous panel
    public void PreviousPanel()
    {
        if (activePaneIndex > 0)
        {
            activePaneIndex--;
            // Enable new panel
            tutorialPanes[activePaneIndex].SetActive(true);
            // Disable previous panel
            tutorialPanes[activePaneIndex + 1].SetActive(false);
        }
    }

    // Close the tutorial
    public void CloseTutorial()
    {
        stateManager.GetComponent<GameState>().EndTutorial();
        // Mark tutorial as complete
        dataManager.GetComponent<LevelStatus>().CompleteTutorial();
    }
}

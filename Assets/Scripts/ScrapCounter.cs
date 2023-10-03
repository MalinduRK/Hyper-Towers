using TMPro;
using UnityEngine;

// All scrap related functions are done through this class
public class ScrapCounter : MonoBehaviour
{
    //--Variables
    private int scrapCount; // Universal scrap count
    private int startingScrap = 10; // Amount of scrap in the beginning of the game
    //--Game objects
    public TextMeshProUGUI scrapCountText; // Scrap text

    private void Start()
    {
        // Assign the starting scrap value to the universal scrap count
        scrapCount = startingScrap;
        // Update UI
        scrapCountText.text = scrapCount.ToString();
    }

    // Add scrap to the scrap pool
    public void AddScrap(int scrapValue)
    {
        scrapCount += scrapValue; // Add the enemy scrap to the scrap pool
        UpdateScrap();
    }

    public void UseScrap(int scrapValue)
    {
        scrapCount -= scrapValue;
        UpdateScrap();
    }

    // Return current scrap value
    public int GetScrap()
    {
        return scrapCount;
    }

    // Update the displayed scrap value in the UI
    public void UpdateScrap()
    {
        scrapCountText.text = scrapCount.ToString();
    }
}

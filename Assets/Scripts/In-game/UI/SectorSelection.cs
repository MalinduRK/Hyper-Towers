using UnityEngine;
using UnityEngine.Audio;

// This script is responsible for handling the selection of a sector in the selection circle

public class SectorSelection : MonoBehaviour
{
    [Header("Assets")]
    public AudioMixer audioMixer;
    [SerializeField] private AudioClip razeSound;

    [Header("Game Objects")]
    public GameObject relatedObject; // Game object related with the selection circle
    private GameObject parentCircle; // Parent selection circle
    private ScrapCounter scrapManager;

    [Header("Components")]
    public Sprite objectSprite;
    private GameInteractivity interactionManager;
    private DetailPanelController detailPanelController;

    [Header("Variables")]
    public string selectionCategory; // Narrows down the selection circle to check in which situation the circle is in (tower built/not built)
    public string assignedAction; // Action assigned for the sector

    private void Start()
    {
        // Get the parent Selection Circle of the sector
        parentCircle = gameObject.transform.parent.gameObject;

        // Assign interaction manager
        interactionManager = GameObject.Find("InteractionManager").GetComponent<GameInteractivity>();

        // Assign detail panel controller
        detailPanelController = GameObject.Find("UIManager").GetComponent<DetailPanelController>();

        // Assign scrap manager
        scrapManager = GameObject.Find("ScrapManager").GetComponent<ScrapCounter>();
    }

    private void OnMouseEnter()
    {
        // Higlight sector

        // TODO: The signal should always be sent to the detailPanelController, and the detailPanelController should check and decide on the action. This is a temporary solution
        if (assignedAction != string.Empty && selectionCategory != "manage") // Only send signal when hovering over unlocked sector
        {
            Debug.Log($"Assigned action: {assignedAction} || Selection category: {selectionCategory}");
            // Pass tower reference and sprite to detail panel controller
            detailPanelController.referenceObjectName = assignedAction;
            detailPanelController.referenceSprite = objectSprite;

            // Notify detail panel controller that the mouse is hovering over a sector
            detailPanelController.isHoveringOverSector = true;
        }
    }

    private void OnMouseExit()
    {
        // Notify detail panel controller that the mouse is not hovering over a sector
        detailPanelController.isHoveringOverSector = false;

        // Notify detail panel controller that the mouse is not hovering over a built tower
        detailPanelController.isHoveringOverTower = false;

        // Reset tier ID upon exiting sector circle
        detailPanelController.tierId = 0;
    }

    private void OnMouseDown() // Clicking on a sector
    {
        // Switch statement is carried out in nested format to minimize the search time for the correct action
        switch (selectionCategory)
        {
            case "towers": // Build tower
                relatedObject.GetComponent<TowerPlacement>().BuildTower(assignedAction);
                break;

            case "manage": // Manage tower (upgrade/raze)
                switch (assignedAction)
                {
                    case "upgrade": // Upgrade tower
                        // Check scrap
                        int currentScrap = scrapManager.GetScrap();
                        // Get reference to TowerStats script
                        TowerStats towerStats = relatedObject.GetComponent<TowerStats>();
                        // Get upgrade cost of the next tower tier
                        int currentTier = towerStats.currentTier; // Get current tier
                        int upgradeCost = towerStats.towerData.tiers[currentTier + 1].cost;
                        // Upgrade tower
                        if (upgradeCost <= currentScrap) // Tower upgradeable
                        {
                            // Change tier data of the tower
                            towerStats.tierData = towerStats.towerData.tiers[++currentTier];
                            towerStats.currentTier = currentTier; // Set new tier

                            // Change tower sprite
                            relatedObject.GetComponent<SpriteRenderer>().sprite = towerStats.towerSprites[currentTier];

                            // Change tower range indicator
                            GameObject towerRange = relatedObject.transform.Find("TowerRange").gameObject;
                            float newRange = towerStats.towerData.tiers[currentTier].range;
                            towerRange.transform.localScale = newRange * Vector3.one;

                            // Use scrap
                            scrapManager.UseScrap(upgradeCost);

                            // Change firerate
                            relatedObject.GetComponent<ProjectileSpawner>().firerate = towerStats.tierData.firerate;
                        }
                        break;

                    case "raze": // Destroy tower
                        // Get tower plot
                        GameObject towerPlot = transform.parent.parent.gameObject;
                        // Send signal that the tower is no longer built on it
                        towerPlot.GetComponent<TowerPlacement>().towerBuilt = false;

                        // Return scrap
                        int scrapValue = relatedObject.GetComponent<TowerStats>().scrapValue; // Get scrap value of the tower
                        scrapManager.AddScrap(scrapValue);

                        // Play tower raze sound
                        float randVolume = Random.Range(0.8f, 1); // Add random volume
                        audioMixer.GetFloat("sfxVolume", out float currentSFXVolume); // Get sfx volume
                        
                        // Convert audio mixer volume to actual volume units of the audio source (the volume sliders in settings go from 0 to -20)
                        float sfxVolumePercent = 0; // Set volume to 0 if audio mixer is at -80db
                        if (currentSFXVolume != -80) // If not, set volume
                        {
                            sfxVolumePercent = (currentSFXVolume + 20) / 20;
                        }

                        float volume = randVolume * sfxVolumePercent;
                        //Debug.Log($"current sfx: {currentSFXVolume} | converted sfx : {sfxVolumePercent} | volume: {volume}");
                        AudioSource.PlayClipAtPoint(razeSound, transform.position, volume);

                        Destroy(relatedObject);
                        break;
                }
                break;
        }

        // Destroy selection circle
        Destroy(parentCircle);

        // Re-enable interactions with outside objects
        interactionManager.EnableInteractions();

        // Notify other scripts that the selection menu is closed
        detailPanelController.isSelectionMenuOpen = false;

        // Notify detail panel controller that the mouse is not hovering over a sector
        detailPanelController.isHoveringOverSector = false;
    }
}

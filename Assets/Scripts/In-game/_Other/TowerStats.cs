using UnityEngine;

// This script handles all the tower data from the moment it is decided to being built. All upgraded and changed values will also be handled and updated here to be accessible to any other script
public class TowerStats : MonoBehaviour
{
    [Header("Assets")]
    public Sprite[] towerSprites;

    [Header("Variables")]
    public TowerData towerData; // All tower data is sent to this variable on creation
    public TierData tierData; // Data related to the current tier of the tower
    public int scrapValue; // Current scrap value of the tower (i.e: Scrap value is the total sum of the tower cost & upgrades)
    public int currentTier = 0; // The current tier the tower is on (i.e: upgrade level). It's always tier 0 at the start
}

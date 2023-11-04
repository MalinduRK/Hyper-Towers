using UnityEngine;

// This script handles all the tower data from the moment it is decided to being built. All upgraded and changed values will also be handled and updated here to be accessible to any other script
public class TowerStats : MonoBehaviour
{
    [Header("Variables")]
    public TowerData towerData; // All tower data is sent to this variable on creation
}

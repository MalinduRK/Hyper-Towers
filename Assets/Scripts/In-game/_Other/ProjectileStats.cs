using UnityEngine;

public class ProjectileStats : MonoBehaviour
{
    [Header("Game Object")]
    public GameObject targetEnemy;

    [Header("Variables")]
    public TowerStats towerStats;
    public float velocity;
    public float damage;
    public float firerate;

    private void Start()
    {
        // Assign data required for the projectile from towerData
        velocity = towerStats.tierData.projectile_velocity;
        damage = towerStats.tierData.damage;
        firerate = towerStats.tierData.firerate;
    }
}

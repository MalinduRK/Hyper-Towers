using UnityEngine;

public class ProjectileStats : MonoBehaviour
{
    [Header("Game Object")]
    public GameObject targetEnemy;

    [Header("Variables")]
    public TowerData towerData;
    public float velocity;
    public float damage;
    public float firerate;

    private void Start()
    {
        // Assign data required for the projectile from towerData
        velocity = towerData.projectile_velocity;
        damage = towerData.damage;
        firerate = towerData.firerate;
    }
}

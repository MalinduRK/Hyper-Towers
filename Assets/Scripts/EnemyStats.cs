using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    //--Variables
    private int enemyMaxHP; // Max HP of enemy
    private int enemyHP; // Current HP of enemy
    private int hitDamage; // Damage per hit (temporary)

    // Start is called before the first frame update
    void Start()
    {
        enemyMaxHP = 10;
        enemyHP = 10;
        hitDamage = 5;
    }

    public void Hit()
    {
        // Damage enemy
        enemyHP -= hitDamage;

        if (enemyHP <= 0) // Enemy is dead
        {
            Destroy(gameObject);
        }
    }
}

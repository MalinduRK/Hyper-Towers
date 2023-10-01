using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    //--Variables
    private int enemyMaxHP; // Max HP of enemy
    private int enemyHP; // Current HP of enemy
    private int hitDamage; // Damage per hit (temporary)
    private float lengthFactor; // Factor which represents 1 unit of HP in terms of length of the health bar
    //--Game objects
    private GameObject currentHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        enemyMaxHP = 10;
        enemyHP = 10;
        hitDamage = 5;

        // Find and assign health bar
        GameObject healthBar = transform.Find("HealthBar(Clone)").gameObject;
        currentHealthBar = healthBar.transform.Find("CurrentHealthBar").gameObject;
        Vector3 scale = currentHealthBar.transform.localScale;

        // Set the length factor
        lengthFactor = scale.x / enemyMaxHP;
        // Multiply any HP unit by the length factor to get the actual length of the health bar

        Debug.Log($"Length factor: {lengthFactor}");
    }

    public void Hit()
    {
        // Damage enemy
        enemyHP -= hitDamage;

        if (enemyHP <= 0) // Enemy is dead
        {
            Destroy(gameObject);
        }
        else
        {
            // Update enemy health bar
            Vector3 newScale = currentHealthBar.transform.localScale;
            newScale.x = enemyHP * lengthFactor ;
            currentHealthBar.transform.localScale = newScale;
        }
    }
}

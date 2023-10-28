using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Game Objects")]
    private GameObject currentHealthBar;
    //private GameObject scrapManager; // Reference to the scrap manager

    [Header("Components")]
    private ScrapCounter scrapCounter; //  Reference to the ScrapCounter script

    [Header("Variables")]
    private int enemyMaxHP; // Max HP of enemy
    private int enemyHP; // Current HP of enemy
    private int hitDamage; // Damage per hit (temporary)
    private float lengthFactor; // Factor which represents 1 unit of HP in terms of length of the health bar
    private int enemyScrapValue = 1; // The amount of scrap recieved for destroying one enemy

    // Start is called before the first frame update
    private void Start()
    {
        enemyMaxHP = 10;
        enemyHP = 10;
        hitDamage = 3;

        // Find and assign health bar
        currentHealthBar = gameObject.transform.Find("Canvas").Find("HealthBar").gameObject;

        Vector3 scale = currentHealthBar.transform.localScale;

        // Find scrap manager and assign scrapCounter
        GameObject scrapManager = GameObject.Find("ScrapManager");
        scrapCounter = scrapManager.GetComponent<ScrapCounter>();

        // Set the length factor
        lengthFactor = scale.x / enemyMaxHP;
        // Multiply any HP unit by the length factor to get the actual length of the health bar

        //Debug.Log($"Length factor: {lengthFactor}");
    }

    public void Hit()
    {
        // Damage enemy
        enemyHP -= hitDamage;

        if (enemyHP <= 0) // Enemy is dead
        {
            Destroy(gameObject);
            // Add 1 scrap to the total scrap count
            scrapCounter.AddScrap(enemyScrapValue);
        }
        else
        {
            // Update enemy health bar
            Vector3 newScale = currentHealthBar.transform.localScale;
            newScale.x = enemyHP * lengthFactor;
            currentHealthBar.transform.localScale = newScale;
        }
    }
}

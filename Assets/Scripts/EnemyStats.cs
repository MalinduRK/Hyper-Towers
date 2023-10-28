using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Game Objects")]
    private GameObject currentHealthBar;
    //private GameObject scrapManager; // Reference to the scrap manager

    [Header("Components")]
    private ScrapCounter scrapCounter; //  Reference to the ScrapCounter script
    private DataReader dataReader;

    [Header("Variables")]
    public EnemyData enemyData = new EnemyData();
    private int enemyMaxHP; // Max HP of enemy
    private int hitDamage; // Damage per hit (temporary)
    private float lengthFactor; // Factor which represents 1 unit of HP in terms of length of the health bar

    // Start is called before the first frame update
    private void Start()
    {
        // Get the name of the current GameObject (It's always in the format "EnemyX(Clone)")
        string gameObjectName = gameObject.name;

        // Remove "(Clone)" from the name
        string enemyName = gameObjectName.Replace("(Clone)", "");

        // Assign data reader
        GameObject dataReaderObject = GameObject.Find("DataManager");
        dataReader = dataReaderObject.GetComponent<DataReader>();

        // Read and assign enemy data
        enemyData = dataReader.ReadEnemyData(enemyName);

        // Store enemy max HP for future reference
        enemyMaxHP = enemyData.health;

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

    public void Hit() // TODO: Take hit damage as parameter
    {
        // Damage enemy
        enemyData.health -= hitDamage;

        if (enemyData.health <= 0) // Enemy is dead
        {
            Destroy(gameObject);
            // Add 1 scrap to the total scrap count
            scrapCounter.AddScrap(enemyData.scrap_value);
        }
        else
        {
            // Update enemy health bar
            Vector3 newScale = currentHealthBar.transform.localScale;
            newScale.x = enemyData.health * lengthFactor;
            currentHealthBar.transform.localScale = newScale;
        }
    }
}

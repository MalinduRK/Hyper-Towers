using UnityEngine;

public class TowerStats : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private TextAsset towerJson; // Reference to tower_data.json file

    [Header("Variables")]
    private TowerJsonData jsonData;

    private void Start()
    {
        // Deserialize the JSON data
        jsonData = JsonUtility.FromJson<TowerJsonData>(towerJson.text);

        // Find the tower with the id
        TowerData tower = jsonData.towers.Find(t => t.id == "Tower1");

        if (tower != null)
        {
            // You now have the TowerData object for "Tower1"
            Debug.Log("Tower Name: " + tower.name);
            Debug.Log("Range: " + tower.range);
            // Access other properties as needed.
        }
        else
        {
            Debug.Log("Tower with id 'Tower1' not found.");
        }
    }

    private void Update()
    {
        
    }
}

using UnityEngine;

public class DataReader : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private TextAsset towerJson; // Reference to tower_data.json file

    [Header("Variables")]
    private TowerJsonData jsonData;

    public TowerData ReadTowerData(string towerId)
    {
        // Deserialize the JSON data
        jsonData = JsonUtility.FromJson<TowerJsonData>(towerJson.text);

        // Find the tower with the id
        TowerData tower = jsonData.towers.Find(t => t.id == towerId);

        // Create variable to store and return tower data
        TowerData towerData = new TowerData();

        // Assign tower data to public variables
        if (tower != null)
        {
            towerData.id = tower.id;
            towerData.name = tower.name;
            towerData.description = tower.description;
            towerData.range = tower.range;
            towerData.damage = tower.damage;
            towerData.firerate = tower.firerate;
            towerData.cost = tower.cost;
            towerData.scrap_value = tower.scrap_value;
        }
        else
        {
            Debug.Log($"Tower with id '{towerId}' not found.");
            return null;
        }

        return towerData;
    }
}

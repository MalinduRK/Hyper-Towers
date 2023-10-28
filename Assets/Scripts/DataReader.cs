using System.Collections.Generic;
using UnityEngine;

public class DataReader : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private TextAsset waveJson; // Reference to wave_data.json file
    [SerializeField] private TextAsset towerJson; // Reference to tower_data.json file
    [SerializeField] private TextAsset enemyJson; // Reference to enemy_data.json file

    public List<WaveData> ReadWaveData(int currentLevel)
    {
        // Deserialize the JSON data into a C# object.
        WaveJsonData jsonData = JsonUtility.FromJson<WaveJsonData>(waveJson.text);

        // Access data specific to the current level
        LevelData levelData = jsonData.levels[currentLevel];

        List<WaveData> waveData; // Wave data for each wave

        // Access wave data for the current level
        waveData = levelData.waves;

        return waveData;
    }

    public TowerData ReadTowerData(string towerId)
    {
        // Deserialize the JSON data
        TowerJsonData jsonData = JsonUtility.FromJson<TowerJsonData>(towerJson.text);

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

    public EnemyData ReadEnemyData(string enemyId)
    {
        // Deserialize the JSON data
        EnemyJsonData jsonData = JsonUtility.FromJson<EnemyJsonData>(enemyJson.text);

        // Find the enemy with the id
        EnemyData enemy = jsonData.enemies.Find(e => e.id == enemyId);

        // Create variable to store and return enemy data
        EnemyData enemyData = new EnemyData();

        // Assign enemy data to public variables
        if (enemy != null)
        {
            enemyData.id = enemy.id;
            enemyData.name = enemy.name;
            enemyData.health = enemy.health;
            enemyData.scrap_value = enemy.scrap_value;
            enemyData.speed = enemy.speed;
            enemyData.damage = enemy.damage;
        }
        else
        {
            Debug.Log($"Enemy with id '{enemyId}' not found.");
            return null;
        }

        return enemyData;
    }
}

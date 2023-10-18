using System.Collections.Generic;

//--Wave Data

[System.Serializable]
public class WaveData
{
    public int enemy_count;
    public float spawn_interval;
    public string[] enemies;
    public string pattern;
}

[System.Serializable]
public class LevelData
{
    public List<WaveData> waves;
}

[System.Serializable]
public class WaveJsonData
{
    public List<LevelData> levels;
}

//--Tower Data

[System.Serializable]
public class TowerData
{
    public string id;
    public string name;
    public string description;
    public float range;
    public float damage;
    public float firerate;
    public int cost;
    public int scrap_value;
}

[System.Serializable]
public class TowerJsonData
{
    public List<TowerData> towers;
}

//--Enemy Data

[System.Serializable]
public class EnemyData
{
    public string id;
    public string name;
    public int health;
    public int scrap_value;
    public float speed;
    public int damage;
}

[System.Serializable]
public class EnemyJsonData
{
    public List<EnemyData> enemies;
}
using JetBrains.Annotations;
using System.Collections.Generic;

//--Wave Data

[System.Serializable]
public class WaveData
{
    public int enemy_count;
    public float spawn_interval;
    public string[] enemies;
    public string pattern;
    public int burst_count;
    public float burst_delay;
}

[System.Serializable]
public class LevelData
{
    public int starting_scrap;
    public List<string> enemy_types;
    public List<string> tower_types;
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
    public List<TierData> tiers;
}

[System.Serializable]
public class TowerJsonData
{
    public List<TowerData> towers;
}

[System.Serializable]
public class TierData
{
    public string tier_id;
    public float range;
    public float turn_rate;
    public float damage;
    public float firerate;
    public float projectile_velocity;
    public int cost;
}

//--Enemy Data

[System.Serializable]
public class EnemyData
{
    public string id;
    public string name;
    public float health;
    public int scrap_value;
    public float speed;
    public int damage;
}

[System.Serializable]
public class EnemyJsonData
{
    public List<EnemyData> enemies;
}
using System.Collections.Generic;

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
public class JsonData
{
    public List<LevelData> levels;
}

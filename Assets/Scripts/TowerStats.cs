using UnityEngine;

// This script handles all the tower data from the moment it is decided to being built. All upgraded and changed values will also be handled and updated here to be accessible to any other script
public class TowerStats : MonoBehaviour
{
    [Header("Components")]
    private DataReader dataReader;

    [Header("Variables")]
    public TowerData towerData = new TowerData();

    private void Start()
    {
        // Get the name of the current GameObject (It's always in the format "TowerX(Clone)")
        string gameObjectName = gameObject.name;

        // Remove "(Clone)" from the name
        string towerName = gameObjectName.Replace("(Clone)", "");

        // Assign data reader
        GameObject dataReaderObject = GameObject.Find("DataManager");
        dataReader = dataReaderObject.GetComponent<DataReader>();

        // Read and assign tower data
        towerData = dataReader.ReadTowerData(towerName);
    }
}

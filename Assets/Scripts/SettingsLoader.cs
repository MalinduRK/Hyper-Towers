using UnityEngine;

// This class will load settings when loading the main menu
public class SettingsLoader : MonoBehaviour
{
    private void Start()
    {
        // Load settings
        //Debug.Log("Loading settings");
        gameObject.GetComponent<SettingsReaderWriter>().LoadSettings();
    }
}

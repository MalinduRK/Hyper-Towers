using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;

    private void Start()
    {
        // Disable popup panel at start
        popupPanel.SetActive(false);
    }

    public void OpenPopup()
    {
        popupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}

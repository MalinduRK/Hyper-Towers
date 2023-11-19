using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private GameObject exitToMenuPopup;
    [SerializeField] private GameObject quitToDesktopPopup;

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

    public void ExitToMainMenuPopup()
    {
        OpenPopup();
        quitToDesktopPopup.SetActive(false);
        exitToMenuPopup.SetActive(true);
    }

    public void QuitToDesktopPopup()
    {
        OpenPopup();
        exitToMenuPopup.SetActive(false);
        quitToDesktopPopup.SetActive(true);
    }
}

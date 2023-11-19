using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI notificationText;

    public void ClearText()
    {
        notificationText.text = string.Empty;
    }

    public void WaveCompleteNotifier()
    {
        string notificationString = "Wave complete";
        ShowNotification(notificationString);
    }

    public void NextWaveNotifier(int waveNumber)
    {
        string notificationString = "Wave " + waveNumber;
        ShowNotification(notificationString);
    }

    public void FinalWaveNotifier()
    {
        string notificationString = "Final wave";
        ShowNotification(notificationString);
    }

    public void ShowNotification(string notificationString)
    {
        notificationPanel.SetActive(true);
        notificationText.text = notificationString;
        StartCoroutine(HideNotification());
    }

    private IEnumerator HideNotification()
    {
        // Wait for 3 seconds and then disable panel
        yield return new WaitForSeconds(3f);

        ClearText();
        notificationPanel.SetActive(false);
    }
}

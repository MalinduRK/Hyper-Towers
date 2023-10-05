using TMPro;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
    private TextMeshProUGUI notificationText;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        // Assign TMP component
        notificationText = GetComponent<TextMeshProUGUI>();
        // Set starting text
        notificationText.text = "Press space to start";
        // Assign animator component
        _animator = GetComponent<Animator>();
    }

    public void ClearText()
    {
        notificationText.text = null;
    }

    public void DisableAnimations()
    {
        _animator.enabled = false;
    }
}

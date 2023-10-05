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

    public void NextWaveNotifier()
    {
        notificationText.text = "Press space to start next wave";
        EnableAnimations();
    }

    public void FinalWaveNotifier()
    {
        notificationText.text = "Final wave";
        EnableAnimations();
    }

    public void EnableAnimations()
    {
        _animator.enabled = true;
    }

    public void DisableAnimations()
    {
        _animator.enabled = false;
    }
}

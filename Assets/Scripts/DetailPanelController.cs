using UnityEngine;

public class DetailPanelController : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject detailPanel;

    [Header("Components")]
    private RectTransform panelRectTransform; // Rect Transform of the detail panel

    [Header("Variables")]
    public bool isSelectionMenuOpen = false;
    public bool isHoveringOverSector = false;
    private float centerScreenY = 0.5f;
    private float panelY;

    private void Start()
    {
        // Disable panel at start
        detailPanel.SetActive(false);

        // Assign rect transform
        panelRectTransform = detailPanel.GetComponent<RectTransform>();

        // Get the Y position of the panel
        panelY = panelRectTransform.localPosition.y;
    }

    private void Update()
    {
        if (isSelectionMenuOpen && isHoveringOverSector)
        {
            // Enable detail panel
            detailPanel.SetActive(true);
            RelocatePanel();
        }
        else
        {
            // Disable detail panel
            detailPanel.SetActive(false);
        }
    }

    // Function to relocate detail panel based on mouse position
    private void RelocatePanel()
    {
        // Get the mouse position in screen coordinates.
        Vector2 mousePosition = Input.mousePosition;

        // Calculate the normalized Y position based on the center of the screen.
        float normalizedY = mousePosition.y / Screen.height;

        // Determine the anchor based on the mouse position.
        if (normalizedY >= centerScreenY)
        {
            // Mouse is above or at the center, anchor to the bottom.
            SetAnchorToBottom();
        }
        else
        {
            // Mouse is below the center, anchor to the top.
            SetAnchorToTop();
        }
    }

    private void SetAnchorToTop()
    {
        if (panelRectTransform != null)
        {
            panelRectTransform.anchorMin = new Vector2(0, 1);
            panelRectTransform.anchorMax = new Vector2(1, 1);
            // Change Y position
            Vector3 newPosition = panelRectTransform.localPosition;
            newPosition.y = -panelY; // Set the new position
            panelRectTransform.localPosition = newPosition;
        }
    }

    private void SetAnchorToBottom()
    {
        if (panelRectTransform != null)
        {
            panelRectTransform.anchorMin = new Vector2(0, 0);
            panelRectTransform.anchorMax = new Vector2(1, 0);
            // Change Y position
            Vector3 newPosition = panelRectTransform.localPosition;
            newPosition.y = panelY; // Set the new position
            panelRectTransform.localPosition = newPosition;
        }
    }
}

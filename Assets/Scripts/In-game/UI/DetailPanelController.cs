using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanelController : MonoBehaviour
{
    [Header("Game Objects")]
    public string referenceObjectName; // The name of object that the detail panel is currently showing information on
    [SerializeField] private GameObject detailPanelHolder; // Container of detail panel
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private GameObject dataReaderObject;

    [Header("Components")]
    public Sprite referenceSprite; // The sprite of the reference object
    private RectTransform panelRectTransform; // Rect Transform of the detail panel

    [Header("Scripts")]
    private DataReader dataReader;

    [Header("Variables")]
    public bool isSelectionMenuOpen = false;
    public bool isHoveringOverSector = false;
    public bool isHoveringOverTower = false;
    public int tierId = 0; // Set default tier to tier 0
    private float centerScreenY = 0.5f;
    private float panelY;

    private void Start()
    {
        // Disable panel at start
        detailPanelHolder.SetActive(false);

        // Assign rect transform
        panelRectTransform = detailPanelHolder.GetComponent<RectTransform>();

        // Get the Y position of the panel
        panelY = panelRectTransform.localPosition.y;

        // Assign data reader
        dataReader = dataReaderObject.GetComponent<DataReader>();
    }

    private void Update()
    {
        if (isSelectionMenuOpen && isHoveringOverSector) // Ignore hovering over other objects when selection circle is open
        {
            // Enable and update detail panel
            UpdatePanel();
            RelocatePanel();
        }
        else if (isSelectionMenuOpen && isHoveringOverTower) // Mouse is hovering over a built tower
        {
            // Enable and update detail panel
            UpdatePanel();
            RelocatePanel();
        }
        else
        {
            // Disable detail panel
            detailPanelHolder.SetActive(false);
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

    // Function to update data inside the detail panel
    private void UpdatePanel()
    {
        // Read data from data reader
        TowerData towerData = dataReader.ReadTowerData(referenceObjectName);

        if (towerData == null)
        {
            return;
        }

        // Enable detail panel
        detailPanelHolder.SetActive(true);

        // Assign values from the reference object to UI elements
        detailPanel.transform.Find("Image").GetComponent<Image>().sprite = referenceSprite;
        detailPanel.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = towerData.name;
        detailPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = towerData.description;

        // Find the "StatsPanel" object
        Transform statsPanel = detailPanel.transform.Find("StatsPanel");

        if (statsPanel != null)
        {
            // Find the "StatsPanelTop" object inside the "StatsPanel" child
            Transform statsPanelTop = statsPanel.Find("StatsPanelTop");

            if (statsPanelTop != null)
            {
                // Find the "Damage" child object inside the "StatsPanelTop" child
                Transform damage = statsPanelTop.Find("Damage");

                if (damage != null)
                {
                    // Find the "DamageValue" object inside the "Damage" child
                    Transform damageValue = damage.Find("DamageValue");

                    if (damageValue != null)
                    {
                        // Update damage value text
                        TextMeshProUGUI damageText = damageValue.GetComponent<TextMeshProUGUI>();
                        damageText.text = towerData.tiers[tierId].damage.ToString();
                    }
                }

                // Find the "Firerate" child object inside the "StatsPanelTop" child
                Transform firerate = statsPanelTop.Find("Firerate");

                if (firerate != null)
                {
                    // Find the "FirerateValue" object inside the "Firerate" child
                    Transform firerateValue = firerate.Find("FirerateValue");

                    if (firerateValue != null)
                    {
                        // Update firerate value text
                        TextMeshProUGUI firerateText = firerateValue.GetComponent<TextMeshProUGUI>();
                        firerateText.text = towerData.tiers[tierId].firerate.ToString();
                    }
                }

                // Find the "Range" child object inside the "StatsPanelTop" child
                Transform range = statsPanelTop.Find("Range");

                if (range != null)
                {
                    // Find the "RangeValue" object inside the "Range" child
                    Transform rangeValue = range.Find("RangeValue");

                    if (rangeValue != null)
                    {
                        // Update range value text
                        TextMeshProUGUI rangeText = rangeValue.GetComponent<TextMeshProUGUI>();
                        rangeText.text = towerData.tiers[tierId].range.ToString();
                    }
                }
            }

            // Find the "StatsPanelBottom" object inside the "StatsPanel" child
            Transform statsPanelBottom = statsPanel.Find("StatsPanelBottom");

            if (statsPanelBottom != null)
            {
                // Find the "RotationSpeed" child object inside the "StatsPanelBottom" child
                Transform rotationSpd = statsPanelBottom.Find("RotationSpeed");

                if (rotationSpd != null)
                {
                    // Find the "RotationSpeedValue" object inside the "RotationSpeed" child
                    Transform rotationSpeedValue = rotationSpd.Find("RotationSpeedValue");

                    if (rotationSpeedValue != null)
                    {
                        // Update RotationSpeedValue text
                        TextMeshProUGUI rotationSpdText = rotationSpeedValue.GetComponent<TextMeshProUGUI>();
                        rotationSpdText.text = towerData.tiers[tierId].turn_rate.ToString();
                    }
                }

                // Find the "ProjectileVelocity" child object inside the "StatsPanelBottom" child
                Transform velocity = statsPanelBottom.Find("ProjectileVelocity");

                if (velocity != null)
                {
                    // Find the "ProjectileVelocityValue" object inside the "ProjectileVelocity" child
                    Transform velocityValue = velocity.Find("ProjectileVelocityValue");

                    if (velocityValue != null)
                    {
                        // Update ProjectileVelocityValue text
                        TextMeshProUGUI velocityText = velocityValue.GetComponent<TextMeshProUGUI>();
                        velocityText.text = towerData.tiers[tierId].projectile_velocity.ToString();
                    }
                }

                // Find the "Scrap" child object inside the "StatsPanelBottom" child
                Transform scrap = statsPanelBottom.Find("Scrap");

                if (scrap != null)
                {
                    // Find the "ScrapValue" object inside the "Scrap" child
                    Transform scrapValue = scrap.Find("ScrapValue");

                    if (scrapValue != null)
                    {
                        // Update scrap value text
                        TextMeshProUGUI scrapText = scrapValue.GetComponent<TextMeshProUGUI>();
                        scrapText.text = towerData.tiers[tierId].cost.ToString();
                    }
                }
            }
        }
    }

    private void SetAnchorToTop()
    {
        if (panelRectTransform != null)
        {
            panelRectTransform.anchorMin = new Vector2(0, 1);
            panelRectTransform.anchorMax = new Vector2(1, 1);
            panelRectTransform.pivot = new Vector2(0.5f, 1);
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
            panelRectTransform.pivot = new Vector2(0.5f, 0);
            // Change Y position
            Vector3 newPosition = panelRectTransform.localPosition;
            newPosition.y = panelY; // Set the new position
            panelRectTransform.localPosition = newPosition;
        }
    }
}

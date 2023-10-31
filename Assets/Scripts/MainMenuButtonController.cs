using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI text; // Text component of this object
    private InterfaceAudioHandler interfaceAudioManager;

    [Header("Variables")]
    [SerializeField] private Color hoverColor = Color.red;
    [SerializeField] private float transitionDuration = 0.5f; // Duration of color transition
    private Color originalColor;
    private Color targetColor;
    private Coroutine colorTransitionCoroutine;

    private void Start()
    {
        // Find interface audio source
        interfaceAudioManager = GameObject.Find("PersistentAudioManager").GetComponent<InterfaceAudioHandler>();

        originalColor = text.color;
        targetColor = originalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        interfaceAudioManager.PlayMenuButtonHoverSound();
        targetColor = hoverColor;
        StartColorTransition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetColor = originalColor;
        StartColorTransition();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        interfaceAudioManager.PlayMenuClickSound();
    }

    private void StartColorTransition()
    {
        if (colorTransitionCoroutine != null)
        {
            StopCoroutine(colorTransitionCoroutine);
        }
        colorTransitionCoroutine = StartCoroutine(TransitionColor());
    }

    private IEnumerator TransitionColor()
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            text.color = Color.Lerp(text.color, targetColor, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        text.color = targetColor;
    }
}

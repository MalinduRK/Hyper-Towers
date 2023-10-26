using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI text; // Text component of this object
    private AudioSource audioSource;

    [SerializeField] private Color hoverColor = Color.red;
    [SerializeField] private float transitionDuration = 0.5f; // Duration of color transition

    private Color originalColor;
    private Color targetColor;
    private Coroutine colorTransitionCoroutine;

    private void Start()
    {
        // Assign audio source
        audioSource = gameObject.GetComponent<AudioSource>();

        originalColor = text.color;
        targetColor = originalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.Play();
        targetColor = hoverColor;
        StartColorTransition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetColor = originalColor;
        StartColorTransition();
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
using UnityEngine;
using UnityEngine.EventSystems;

public class BackButtonController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [Header("Components")]
    private InterfaceAudioHandler interfaceAudioManager;

    private void Start()
    {
        // Find interface audio source
        interfaceAudioManager = GameObject.Find("PersistentAudioManager").GetComponent<InterfaceAudioHandler>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        interfaceAudioManager.PlayMenuBackClickSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Add hover effect
    }
}

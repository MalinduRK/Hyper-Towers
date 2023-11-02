using UnityEngine;
using UnityEngine.EventSystems;

public class BackButtonController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [Header("Assets")]
    [SerializeField] private AudioClip menuBackButtonClickSound;

    [Header("Components")]
    private InterfaceAudioHandler interfaceAudioManager;

    private void Start()
    {
        // Find interface audio source
        interfaceAudioManager = GameObject.Find("PersistentAudioManager").GetComponent<InterfaceAudioHandler>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        interfaceAudioManager.PlayClip(menuBackButtonClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Add hover effect
    }
}

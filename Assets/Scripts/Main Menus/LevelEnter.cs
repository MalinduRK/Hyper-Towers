using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEnter : MonoBehaviour, IPointerClickHandler
{
    [Header("Assets")]
    public AudioClip levelEnterSound;

    [Header("Components")]
    private InterfaceAudioHandler interfaceAudioManager; // Persistent audio manager

    private void Start()
    {
        // Find interface audio source
        interfaceAudioManager = GameObject.Find("PersistentAudioManager").GetComponent<InterfaceAudioHandler>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        interfaceAudioManager.PlayClip(levelEnterSound, 0.15f);
    }
}

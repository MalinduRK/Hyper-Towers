using UnityEngine;

public class CloseMenu : MonoBehaviour
{
    [Header("Game Objects")]
    private GameObject parentCircle; // Parent selection circle
    private GameInteractivity interactionManager;

    private void Start()
    {
        // Get the parent Selection Circle
        parentCircle = gameObject.transform.parent.gameObject;

        // Assign interaction manager
        interactionManager = GameObject.Find("InteractionManager").GetComponent<GameInteractivity>();
    }

    private void OnMouseDown()
    {
        CloseDebug("Selection circle closed");

        // Destroy selection circle
        Destroy(parentCircle);

        // Re-enable interactions with outside objects
        interactionManager.EnableInteractions();
    }

    //--Debugs

    [SerializeField] private bool closeDebug;

    private void CloseDebug(string message)
    {
        if (closeDebug)
        {
            Debug.Log(message);
        }
    }
}

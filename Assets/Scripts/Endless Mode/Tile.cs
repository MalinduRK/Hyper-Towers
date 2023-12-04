using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _renderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlight;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter() {
        //_highlight.SetActive(true);
        _renderer.material = _highlight;
    }

    void OnMouseExit()
    {
        //_highlight.SetActive(false);
        _renderer.material = _defaultMaterial;
    }
}
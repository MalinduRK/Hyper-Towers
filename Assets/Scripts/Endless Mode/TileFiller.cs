using UnityEngine;
using UnityEngine.Tilemaps;

public class TileFiller : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase emptyTile;
    public Material highlightMaterial; // Reference to the highlight material
    private Color originalColor;

    private TilemapRenderer tilemapRenderer;
    private TilemapCollider2D tilemapCollider;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        originalColor = tilemapRenderer.material.color;
        //tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
        //tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();
        FillScreenWithTile();
    }

    void FillScreenWithTile()
    {
        // Get the dimensions of the screen in tiles
        BoundsInt bounds = tilemap.cellBounds;

        // Loop through each cell and set the tile
        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            tilemap.SetTile(position, emptyTile);
        }
    }

    /*void Update()
    {
        // Raycast to detect which tile is being hovered
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(mousePos);

        // Check if the mouse is over the tilemap
        if (tilemapCollider.OverlapPoint(mousePos))
        {
            // Highlight the hovered tile
            HighlightTile(cellPosition);
        }
        else
        {
            // Reset the highlighting when not hovering over the tilemap
            ResetHighlight();
        }
    }*/

    private void OnMouseEnter()
    {
        HighlightTile();
    }

    private void OnMouseExit()
    {
        ResetHighlight();
    }

    /*void HighlightTile(Vector3Int position)
    {
        tilemap.SetTileFlags(position, TileFlags.None);

        // Get the tile at the specified position
        TileBase tile = tilemap.GetTile(position);

        // Create a new GameObject for the highlight
        GameObject highlight = new GameObject("Highlight");
        highlight.transform.position = tilemap.GetCellCenterWorld(position);

        // Add a SpriteRenderer to the highlight GameObject
        SpriteRenderer renderer = highlight.AddComponent<SpriteRenderer>();
        renderer.sprite = ((Tile)tile).sprite; // Adjust if using a different type of tile
        renderer.material = highlightMaterial;

        // Adjust the sorting order to render above the tilemap
        renderer.sortingOrder = tilemapRenderer.sortingOrder + 1;
    }*/

    /*void ResetHighlight()
    {
        // Destroy any existing highlight GameObjects
        GameObject highlight = GameObject.Find("Highlight");
        if (highlight != null)
        {
            Destroy(highlight);
        }
    }*/

    private void HighlightTile()
    {
        // Change the color or apply a shader effect to highlight the tile
        tilemapRenderer.material.color = Color.yellow; // You can customize the color or use a different approach
    }

    private void ResetHighlight()
    {
        // Reset the color to the original color
        tilemapRenderer.material.color = originalColor;
    }
}

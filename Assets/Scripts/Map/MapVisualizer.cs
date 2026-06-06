using UnityEngine;

/// <summary>
/// Harita görselleştirme - Unity'de haritayı ekranda çizer
/// </summary>
public class MapVisualizer : MonoBehaviour
{
    private Grid grid;
    private Dictionary<TileType, Color> tileColors = new Dictionary<TileType, Color>();
    private Dictionary<Tile, GameObject> tileGameObjects = new Dictionary<Tile, GameObject>();
    private float tileSize = 1f;

    public void Initialize(Grid gameGrid, float size)
    {
        grid = gameGrid;
        tileSize = size;
        SetupTileColors();
        GenerateTileVisuals();
    }

    /// <summary>
    /// Kare türlerine renk ata
    /// </summary>
    private void SetupTileColors()
    {
        tileColors[TileType.Grass] = new Color(0.2f, 0.8f, 0.2f);      // Yeşil
        tileColors[TileType.Forest] = new Color(0.1f, 0.5f, 0.1f);     // Koyu yeşil
        tileColors[TileType.Mountain] = new Color(0.6f, 0.6f, 0.6f);   // Gri
        tileColors[TileType.Water] = new Color(0.2f, 0.5f, 0.9f);      // Mavi
        tileColors[TileType.Desert] = new Color(0.9f, 0.8f, 0.3f);     // Sarı
        tileColors[TileType.Snow] = new Color(0.95f, 0.95f, 0.95f);    // Beyaz
        tileColors[TileType.Swamp] = new Color(0.5f, 0.6f, 0.3f);      // Kahverengi
    }

    /// <summary>
    /// Harita karelerini görselleştir
    /// </summary>
    private void GenerateTileVisuals()
    {
        GameObject mapContainer = new GameObject("MapContainer");
        mapContainer.transform.parent = transform;

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Tile tile = grid.GetTile(x, y);
                if (tile != null)
                {
                    GameObject tileObj = CreateTileVisual(tile, mapContainer.transform);
                    tileGameObjects[tile] = tileObj;
                }
            }
        }

        Debug.Log("[MapVisualizer] Harita görselleştirildi.");
    }

    /// <summary>
    /// Tek bir kare için visual GameObject oluştur
    /// </summary>
    private GameObject CreateTileVisual(Tile tile, Transform parent)
    {
        GameObject tileObj = new GameObject($"Tile_{tile.X}_{tile.Y}");
        tileObj.transform.parent = parent;
        tileObj.transform.position = tile.WorldPosition + Vector3.up * 0.5f;

        // Cube oluştur
        var meshFilter = tileObj.AddComponent<MeshFilter>();
        var meshRenderer = tileObj.AddComponent<MeshRenderer>();
        var boxCollider = tileObj.AddComponent<BoxCollider>();

        // Mesh
        meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");

        // Material
        Material mat = new Material(Shader.Find("Standard"));
        if (tileColors.TryGetValue(tile.TileType, out Color color))
        {
            mat.color = color;
        }
        meshRenderer.material = mat;

        // Scale
        tileObj.transform.localScale = new Vector3(tileSize, 0.5f, tileSize);

        return tileObj;
    }

    /// <summary>
    /// Kareyi vurgula (seçili kare)
    /// </summary>
    public void HighlightTile(Tile tile, bool highlight)
    {
        if (tileGameObjects.TryGetValue(tile, out GameObject tileObj))
        {
            var renderer = tileObj.GetComponent<MeshRenderer>();
            if (highlight)
            {
                Material highlightMat = new Material(Shader.Find("Standard"));
                highlightMat.color = Color.yellow;
                renderer.material = highlightMat;
            }
            else
            {
                Material mat = new Material(Shader.Find("Standard"));
                if (tileColors.TryGetValue(tile.TileType, out Color color))
                {
                    mat.color = color;
                }
                renderer.material = mat;
            }
        }
    }

    /// <summary>
    /// Kare rengini güncelle
    /// </summary>
    public void UpdateTileColor(Tile tile, Color newColor)
    {
        if (tileGameObjects.TryGetValue(tile, out GameObject tileObj))
        {
            var renderer = tileObj.GetComponent<MeshRenderer>();
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = newColor;
            renderer.material = mat;
        }
    }

    public Dictionary<Tile, GameObject> GetTileGameObjects() => tileGameObjects;
}

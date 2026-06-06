using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Haritayı oluşturur ve başlangıç temelini sağlar
/// </summary>
public class MapGenerator : MonoBehaviour
{
    /// <summary>
    /// Perlin Noise kullanarak rastgele harita oluştur
    /// </summary>
    public static void GenerateMap(Grid grid)
    {
        int width = grid.GetWidth();
        int height = grid.GetHeight();
        float tileSize = grid.GetTileSize();
        float scale = 5f; // Perlin noise ölçeği
        float seed = Random.Range(0f, 100f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Perlin noise değeri al
                float noiseValue = Mathf.PerlinNoise(
                    (x + seed) / scale,
                    (y + seed) / scale
                );

                // Kare türünü belirle
                TileType tileType = GetTileTypeFromNoise(noiseValue);

                // World position hesapla
                Vector3 worldPos = new Vector3(x * tileSize, 0, y * tileSize);

                // Tile oluştur ve grid'e ekle
                Tile tile = new Tile(x, y, tileType, worldPos);
                grid.SetTile(x, y, tile);
            }
        }

        Debug.Log($"[MapGenerator] Harita oluşturuldu: {width}x{height}");
    }

    /// <summary>
    /// Perlin noise değerine göre kare türünü belirle
    /// </summary>
    private static TileType GetTileTypeFromNoise(float noiseValue)
    {
        // noiseValue 0-1 arasında
        if (noiseValue < 0.3f)
            return TileType.Water;
        else if (noiseValue < 0.4f)
            return TileType.Swamp;
        else if (noiseValue < 0.5f)
            return TileType.Desert;
        else if (noiseValue < 0.65f)
            return TileType.Grass;
        else if (noiseValue < 0.8f)
            return TileType.Forest;
        else if (noiseValue < 0.9f)
            return TileType.Mountain;
        else
            return TileType.Snow;
    }

    /// <summary>
    /// Belirli kareleri sahiplendirmek için kullan
    /// </summary>
    public static void ClaimTile(Grid grid, int x, int y, int tribeId)
    {
        Tile tile = grid.GetTile(x, y);
        if (tile != null)
        {
            tile.SetOwner(tribeId);
            Debug.Log($"[MapGenerator] Kare ({x}, {y}) Tribe {tribeId} tarafından sahiplendirildi.");
        }
    }
}

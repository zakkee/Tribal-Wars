using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Geliştirilmiş harita oluşturucu - Birden fazla harita türü desteği
/// </summary>
public class AdvancedMapGenerator : MonoBehaviour
{
    public enum MapType
    {
        Balanced,      // Dengeli harita
        Islands,       // Ada haritası
        Continental,   // Kıta haritası
        Mountains,     // Dağlık harita
        Forest,        // Orman haritası
        Desert         // Çöl haritası
    }

    /// <summary>
    /// İleri harita oluştur
    /// </summary>
    public static void GenerateAdvancedMap(Grid grid, MapType mapType, int seed = -1)
    {
        if (seed == -1)
            seed = Random.Range(0, 1000);

        Random.InitState(seed);

        int width = grid.GetWidth();
        int height = grid.GetHeight();
        float tileSize = grid.GetTileSize();
        float noiseScale = 5f;

        switch (mapType)
        {
            case MapType.Balanced:
                GenerateBalancedMap(grid, width, height, tileSize, noiseScale);
                break;
            case MapType.Islands:
                GenerateIslandMap(grid, width, height, tileSize);
                break;
            case MapType.Continental:
                GenerateContinentalMap(grid, width, height, tileSize, noiseScale);
                break;
            case MapType.Mountains:
                GenerateMountainousMap(grid, width, height, tileSize, noiseScale);
                break;
            case MapType.Forest:
                GenerateForestMap(grid, width, height, tileSize, noiseScale);
                break;
            case MapType.Desert:
                GenerateDesertMap(grid, width, height, tileSize, noiseScale);
                break;
        }

        Debug.Log($"[AdvancedMapGenerator] {mapType} harita türü oluşturuldu!");
    }

    /// <summary>
    /// Dengeli harita oluştur
    /// </summary>
    private static void GenerateBalancedMap(Grid grid, int width, int height, float tileSize, float noiseScale)
    {
        float seed = Random.Range(0f, 100f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise((x + seed) / noiseScale, (y + seed) / noiseScale);
                TileType tileType = GetTileTypeFromNoise(noiseValue, 0.5f); // Dengeli
                SetTile(grid, x, y, tileType, tileSize);
            }
        }
    }

    /// <summary>
    /// Ada haritası oluştur (çok su)
    /// </summary>
    private static void GenerateIslandMap(Grid grid, int width, int height, float tileSize)
    {
        float seed = Random.Range(0f, 100f);
        float noiseScale = 3f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise((x + seed) / noiseScale, (y + seed) / noiseScale);

                // Ada haritasında çok su var
                if (noiseValue < 0.4f)
                    SetTile(grid, x, y, TileType.Water, tileSize);
                else if (noiseValue < 0.5f)
                    SetTile(grid, x, y, TileType.Grass, tileSize);
                else if (noiseValue < 0.65f)
                    SetTile(grid, x, y, TileType.Forest, tileSize);
                else
                    SetTile(grid, x, y, TileType.Mountain, tileSize);
            }
        }
    }

    /// <summary>
    /// Kıta haritası oluştur
    /// </summary>
    private static void GenerateContinentalMap(Grid grid, int width, int height, float tileSize, float noiseScale)
    {
        float seed = Random.Range(0f, 100f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise((x + seed) / noiseScale, (y + seed) / noiseScale);
                TileType tileType = GetTileTypeFromNoise(noiseValue, 0.6f); // Kıta haritası
                SetTile(grid, x, y, tileType, tileSize);
            }
        }
    }

    /// <summary>
    /// Dağlık harita oluştur
    /// </summary>
    private static void GenerateMountainousMap(Grid grid, int width, int height, float tileSize, float noiseScale)
    {
        float seed = Random.Range(0f, 100f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise((x + seed) / noiseScale, (y + seed) / noiseScale);

                if (noiseValue < 0.35f)
                    SetTile(grid, x, y, TileType.Grass, tileSize);
                else if (noiseValue < 0.5f)
                    SetTile(grid, x, y, TileType.Mountain, tileSize);
                else if (noiseValue < 0.7f)
                    SetTile(grid, x, y, TileType.Snow, tileSize);
                else
                    SetTile(grid, x, y, TileType.Snow, tileSize);
            }
        }
    }

    /// <summary>
    /// Orman haritası oluştur
    /// </summary>
    private static void GenerateForestMap(Grid grid, int width, int height, float tileSize, float noiseScale)
    {
        float seed = Random.Range(0f, 100f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise((x + seed) / noiseScale, (y + seed) / noiseScale);

                if (noiseValue < 0.3f)
                    SetTile(grid, x, y, TileType.Water, tileSize);
                else if (noiseValue < 0.45f)
                    SetTile(grid, x, y, TileType.Forest, tileSize);
                else if (noiseValue < 0.6f)
                    SetTile(grid, x, y, TileType.Grass, tileSize);
                else
                    SetTile(grid, x, y, TileType.Forest, tileSize);
            }
        }
    }

    /// <summary>
    /// Çöl haritası oluştur
    /// </summary>
    private static void GenerateDesertMap(Grid grid, int width, int height, float tileSize, float noiseScale)
    {
        float seed = Random.Range(0f, 100f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise((x + seed) / noiseScale, (y + seed) / noiseScale);

                if (noiseValue < 0.4f)
                    SetTile(grid, x, y, TileType.Desert, tileSize);
                else if (noiseValue < 0.6f)
                    SetTile(grid, x, y, TileType.Desert, tileSize);
                else if (noiseValue < 0.75f)
                    SetTile(grid, x, y, TileType.Mountain, tileSize);
                else
                    SetTile(grid, x, y, TileType.Swamp, tileSize);
            }
        }
    }

    /// <summary>
    /// Tile ayarla
    /// </summary>
    private static void SetTile(Grid grid, int x, int y, TileType tileType, float tileSize)
    {
        Vector3 worldPos = new Vector3(x * tileSize, 0, y * tileSize);
        Tile tile = new Tile(x, y, tileType, worldPos);
        grid.SetTile(x, y, tile);
    }

    /// <summary>
    /// Perlin noise değerinden kare türü belirle
    /// </summary>
    private static TileType GetTileTypeFromNoise(float noiseValue, float waterLevel)
    {
        if (noiseValue < waterLevel - 0.15f)
            return TileType.Water;
        else if (noiseValue < waterLevel - 0.05f)
            return TileType.Swamp;
        else if (noiseValue < waterLevel + 0.15f)
            return TileType.Grass;
        else if (noiseValue < waterLevel + 0.3f)
            return TileType.Forest;
        else if (noiseValue < waterLevel + 0.4f)
            return TileType.Desert;
        else if (noiseValue < waterLevel + 0.5f)
            return TileType.Mountain;
        else
            return TileType.Snow;
    }
}

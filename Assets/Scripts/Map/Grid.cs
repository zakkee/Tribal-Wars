using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Harita grid sistemini yönetir
/// </summary>
public class Grid
{
    private Tile[,] tiles;
    private int width;
    private int height;
    private float tileSize;

    public Grid(int width, int height, float tileSize)
    {
        this.width = width;
        this.height = height;
        this.tileSize = tileSize;
        tiles = new Tile[width, height];
    }

    public void SetTile(int x, int y, Tile tile)
    {
        if (IsValidPosition(x, y))
        {
            tiles[x, y] = tile;
        }
    }

    public Tile GetTile(int x, int y)
    {
        if (IsValidPosition(x, y))
        {
            return tiles[x, y];
        }
        return null;
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetTileSize()
    {
        return tileSize;
    }

    /// <summary>
    /// İki konum arasında Manhattan mesafesini hesapla
    /// </summary>
    public int GetDistance(int x1, int y1, int x2, int y2)
    {
        return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
    }

    /// <summary>
    /// Verilen konum etrafındaki tüm kareleri döndür
    /// </summary>
    public List<Tile> GetAdjacentTiles(int x, int y)
    {
        List<Tile> adjacentTiles = new List<Tile>();

        int[,] directions = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

        for (int i = 0; i < 4; i++)
        {
            int newX = x + directions[i, 0];
            int newY = y + directions[i, 1];

            if (IsValidPosition(newX, newY))
            {
                adjacentTiles.Add(tiles[newX, newY]);
            }
        }

        return adjacentTiles;
    }

    /// <summary>
    /// Verilen yarıçap içindeki tüm kareleri döndür
    /// </summary>
    public List<Tile> GetTilesInRange(int centerX, int centerY, int range)
    {
        List<Tile> tilesInRange = new List<Tile>();

        for (int x = centerX - range; x <= centerX + range; x++)
        {
            for (int y = centerY - range; y <= centerY + range; y++)
            {
                if (IsValidPosition(x, y))
                {
                    if (GetDistance(centerX, centerY, x, y) <= range)
                    {
                        tilesInRange.Add(tiles[x, y]);
                    }
                }
            }
        }

        return tilesInRange;
    }
}

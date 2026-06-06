using UnityEngine;

/// <summary>
/// Harita üzerindeki tek bir kareyi temsil eder
/// </summary>
public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public TileType TileType { get; set; }
    public Unit OccupyingUnit { get; set; }
    public int OwnerId { get; set; } = -1; // -1 = sahipsiz
    public Vector3 WorldPosition { get; set; }

    public Tile(int x, int y, TileType tileType, Vector3 worldPosition)
    {
        X = x;
        Y = y;
        TileType = tileType;
        WorldPosition = worldPosition;
        OccupyingUnit = null;
    }

    public bool IsOccupied()
    {
        return OccupyingUnit != null;
    }

    public bool IsOwned()
    {
        return OwnerId != -1;
    }

    public void SetOwner(int ownerId)
    {
        OwnerId = ownerId;
    }

    public bool CanPlace(Unit unit)
    {
        // Su ve dağa birim konulamaz (şimdilik)
        if (TileType == TileType.Water || TileType == TileType.Mountain)
            return false;

        // Eğer zaten birim varsa konulamaz
        if (IsOccupied())
            return false;

        return true;
    }

    public override string ToString()
    {
        return $"Tile({X}, {Y}) - {TileType} - Owner: {OwnerId}";
    }
}

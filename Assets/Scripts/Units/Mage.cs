using UnityEngine;

/// <summary>
/// Büyücü birimi - Alansal saldırı, orta savunma
/// </summary>
public class Mage : Unit
{
    public override void Initialize(int x, int y, int tribe, Grid gameGrid)
    {
        unitType = UnitType.Mage;
        health = 50;
        maxHealth = 50;
        attack = 18;
        defense = 4;
        movementRange = 2;
        attackRange = 2;
        cost = 80;

        base.Initialize(x, y, tribe, gameGrid);
    }

    /// <summary>
    /// Mage'in alansal saldırısını gerçekleştir
    /// </summary>
    public bool AoeAttack(int targetX, int targetY)
    {
        if (hasAttackedThisTurn)
        {
            Debug.Log("[Mage] Bu tur zaten saldırdınız!");
            return false;
        }

        // Mesafe kontrol
        int distance = grid.GetDistance(currentX, currentY, targetX, targetY);
        if (distance > attackRange)
        {
            Debug.Log($"[Mage] Hedef alansal saldırı mesafesinin dışında!");
            return false;
        }

        // 1 kare çapında tüm birimleri saldır
        var affectedTiles = grid.GetTilesInRange(targetX, targetY, 1);
        int totalDamage = 0;

        foreach (var tile in affectedTiles)
        {
            if (tile.IsOccupied() && tile.OccupyingUnit.GetTribeId() != tribeId)
            {
                int damage = CalculateDamage(tile.OccupyingUnit);
                tile.OccupyingUnit.TakeDamage(damage);
                totalDamage += damage;
            }
        }

        hasAttackedThisTurn = true;
        Debug.Log($"[Mage] Alansal saldırı! Toplam hasar: {totalDamage}");
        return true;
    }
}

using UnityEngine;

/// <summary>
/// Süvari birimi - Hızlı hareket, dengeli saldırı
/// </summary>
public class Cavalry : Unit
{
    public override void Initialize(int x, int y, int tribe, Grid gameGrid)
    {
        unitType = UnitType.Cavalry;
        health = 70;
        maxHealth = 70;
        attack = 14;
        defense = 5;
        movementRange = 4;
        attackRange = 1;
        cost = 75;

        base.Initialize(x, y, tribe, gameGrid);
    }
}

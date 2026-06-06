using UnityEngine;

/// <summary>
/// Okçu birimi - Uzun mesafe saldırısı, düşük savunma
/// </summary>
public class Archer : Unit
{
    public override void Initialize(int x, int y, int tribe, Grid gameGrid)
    {
        unitType = UnitType.Archer;
        health = 60;
        maxHealth = 60;
        attack = 15;
        defense = 3;
        movementRange = 2;
        attackRange = 3;
        cost = 60;

        base.Initialize(x, y, tribe, gameGrid);
    }
}

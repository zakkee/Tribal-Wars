using UnityEngine;

/// <summary>
/// Savunmacı birimi - Yüksek savunma, düşük saldırı
/// </summary>
public class Defender : Unit
{
    public override void Initialize(int x, int y, int tribe, Grid gameGrid)
    {
        unitType = UnitType.Defender;
        health = 120;
        maxHealth = 120;
        attack = 8;
        defense = 12;
        movementRange = 1;
        attackRange = 1;
        cost = 70;

        base.Initialize(x, y, tribe, gameGrid);
    }
}

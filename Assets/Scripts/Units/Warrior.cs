using UnityEngine;

/// <summary>
/// Savaşçı birimi - Dengeli saldırı ve savunma
/// </summary>
public class Warrior : Unit
{
    public override void Initialize(int x, int y, int tribe, Grid gameGrid)
    {
        unitType = UnitType.Warrior;
        health = 80;
        maxHealth = 80;
        attack = 12;
        defense = 7;
        movementRange = 2;
        attackRange = 1;
        cost = 50;

        base.Initialize(x, y, tribe, gameGrid);
    }
}

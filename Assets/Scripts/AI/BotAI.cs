using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Bot AI - Yapay zeka ile oyuncu gibi hareket eden aşiret
/// </summary>
public class BotAI : MonoBehaviour
{
    private Tribe botTribe;
    private Grid grid;
    private TurnManager turnManager;
    private AIStrategy strategy;
    private Difficulty difficulty;

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public void Initialize(Tribe tribe, Grid gameGrid, TurnManager turnMgr, Difficulty diff)
    {
        botTribe = tribe;
        grid = gameGrid;
        turnManager = turnMgr;
        difficulty = diff;

        // Zorluk seviyesine göre strateji belirle
        strategy = new AIStrategy(difficulty);

        Debug.Log($"[BotAI] Bot oluşturuldu! Aşiret: {botTribe.tribeName}, Zorluk: {difficulty}");
    }

    /// <summary>
    /// Bot'un turunu oyna
    /// </summary>
    public void PlayTurn()
    {
        if (botTribe == null || botTribe.GetUnitCount() == 0)
        {
            Debug.Log("[BotAI] Bot aşiretinin birimi yok!");
            turnManager.EndTurn();
            return;
        }

        Debug.Log($"[BotAI] {botTribe.tribeName} (Bot) turunu oynuyor...");

        // Birim üret
        ProduceUnits();

        // Birimler ile hamle yap
        MoveAndAttackUnits();

        // Tur bitir
        Invoke(nameof(EndBotTurn), 2f); // 2 saniye bekle
    }

    /// <summary>
    /// Bot birim üret
    /// </summary>
    private void ProduceUnits()
    {
        int goldAvailable = botTribe.GetGold();

        // Kolay mod: Savaşçı üret
        if (difficulty == Difficulty.Easy)
        {
            if (goldAvailable >= 50)
            {
                SpawnUnitAtRandomLocation(UnitType.Warrior);
            }
        }
        // Normal mod: Dengeli seçim
        else if (difficulty == Difficulty.Normal)
        {
            if (goldAvailable >= 75)
            {
                int choice = Random.Range(0, 3);
                switch (choice)
                {
                    case 0: SpawnUnitAtRandomLocation(UnitType.Warrior); break;
                    case 1: SpawnUnitAtRandomLocation(UnitType.Archer); break;
                    case 2: SpawnUnitAtRandomLocation(UnitType.Cavalry); break;
                }
            }
        }
        // Zor mod: Stratejik seçim
        else if (difficulty == Difficulty.Hard)
        {
            if (goldAvailable >= 75)
            {
                // Düşman yakınsa savunmacı, uzak ise okçu üret
                if (IsEnemyNear())
                {
                    SpawnUnitAtRandomLocation(UnitType.Defender);
                }
                else
                {
                    SpawnUnitAtRandomLocation(UnitType.Archer);
                }
            }
        }
    }

    /// <summary>
    /// Rastgele konumda birim oluştur
    /// </summary>
    private void SpawnUnitAtRandomLocation(UnitType unitType)
    {
        List<Tile> ownedTiles = botTribe.GetOwnedTiles();

        if (ownedTiles.Count == 0)
        {
            // Harita üzerinden rastgele konum seç
            int randomX = Random.Range(0, grid.GetWidth());
            int randomY = Random.Range(0, grid.GetHeight());
            botTribe.ProduceUnit(unitType, randomX, randomY);
        }
        else
        {
            // Sahip olunan bir karede birim oluştur
            Tile spawnTile = ownedTiles[Random.Range(0, ownedTiles.Count)];
            botTribe.ProduceUnit(unitType, spawnTile.X, spawnTile.Y);
        }
    }

    /// <summary>
    /// Birimler ile hamle yap ve saldırı et
    /// </summary>
    private void MoveAndAttackUnits()
    {
        List<Unit> botUnits = botTribe.GetUnits();

        foreach (var unit in botUnits)
        {
            if (unit == null) continue;

            // Düşman birimini ara
            Unit targetEnemy = FindNearestEnemy(unit);

            if (targetEnemy != null)
            {
                // Düşmana doğru hareket et
                MoveTowards(unit, targetEnemy.GetX(), targetEnemy.GetY());

                // Eğer yakın ise saldır
                if (CanAttack(unit, targetEnemy))
                {
                    unit.Attack(targetEnemy);
                }
            }
            else
            {
                // Düşman yoksa keşfet veya kaynak topla
                ExploreMap(unit);
            }
        }
    }

    /// <summary>
    /// En yakın düşman birimini bul
    /// </summary>
    private Unit FindNearestEnemy(Unit unit)
    {
        Unit nearestEnemy = null;
        int minDistance = int.MaxValue;

        // Diğer aşiretlerin birimlerini kontrol et
        foreach (var tribe in turnManager.GetAllTribes())
        {
            if (tribe == botTribe) continue;

            foreach (var enemyUnit in tribe.GetUnits())
            {
                if (enemyUnit == null) continue;

                int distance = grid.GetDistance(unit.GetX(), unit.GetY(), enemyUnit.GetX(), enemyUnit.GetY());

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = enemyUnit;
                }
            }
        }

        return nearestEnemy;
    }

    /// <summary>
    /// Birimi hedef konuma doğru hareket ettir
    /// </summary>
    private void MoveTowards(Unit unit, int targetX, int targetY)
    {
        int unitX = unit.GetX();
        int unitY = unit.GetY();

        // Basit A* veya Manhattan mesafesi ile hamle yönünü belirle
        int newX = unitX;
        int newY = unitY;

        if (unitX < targetX) newX++;
        else if (unitX > targetX) newX--;

        if (unitY < targetY) newY++;
        else if (unitY > targetY) newY--;

        // Hareket ettir
        unit.Move(newX, newY);
    }

    /// <summary>
    /// Birim hedefi saldırabilir mi kontrol et
    /// </summary>
    private bool CanAttack(Unit attacker, Unit target)
    {
        int distance = grid.GetDistance(attacker.GetX(), attacker.GetY(), target.GetX(), target.GetY());
        return distance <= attacker.GetAttackRange();
    }

    /// <summary>
    /// Haritayı keşfet
    /// </summary>
    private void ExploreMap(Unit unit)
    {
        // Rastgele bir yöne hareket et
        int randomX = Random.Range(unit.GetX() - 1, unit.GetX() + 2);
        int randomY = Random.Range(unit.GetY() - 1, unit.GetY() + 2);

        if (grid.IsValidPosition(randomX, randomY))
        {
            unit.Move(randomX, randomY);
        }
    }

    /// <summary>
    /// Düşman yakın mı kontrol et
    /// </summary>
    private bool IsEnemyNear()
    {
        List<Unit> botUnits = botTribe.GetUnits();
        int searchRadius = 5;

        foreach (var unit in botUnits)
        {
            if (unit == null) continue;

            foreach (var tribe in turnManager.GetAllTribes())
            {
                if (tribe == botTribe) continue;

                foreach (var enemyUnit in tribe.GetUnits())
                {
                    int distance = grid.GetDistance(unit.GetX(), unit.GetY(), enemyUnit.GetX(), enemyUnit.GetY());
                    if (distance <= searchRadius)
                        return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Bot turunu bitir
    /// </summary>
    private void EndBotTurn()
    {
        Debug.Log($"[BotAI] {botTribe.tribeName} turunu bitirdi.");
        turnManager.EndTurn();
    }
}

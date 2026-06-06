using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Aşiret sınıfı. Her oyuncu bir aşireti kontrol eder.
/// </summary>
public class Tribe : MonoBehaviour
{
    public int tribeId;
    public string tribeName;
    public TribeType tribeType;
    public Color tribeColor;

    [SerializeField] private int gold = 500;
    [SerializeField] private int diamonds = 100;
    [SerializeField] private int population = 50;
    [SerializeField] private int maxPopulation = 100;

    private List<Unit> units = new List<Unit>();
    private List<Tile> ownedTiles = new List<Tile>();
    private Grid grid;

    public void Initialize(int id, string name, TribeType type, Color color, Grid gameGrid)
    {
        tribeId = id;
        tribeName = name;
        tribeType = type;
        tribeColor = color;
        grid = gameGrid;

        Debug.Log($"[Tribe] {tribeName} ({tribeType}) aşireti oluşturuldu! ID: {tribeId}");
    }

    /// <summary>
    /// Aşirete bir birim ekle
    /// </summary>
    public void AddUnit(Unit unit)
    {
        units.Add(unit);
        Debug.Log($"[Tribe] {tribeName} - {unit.GetUnitType()} birimi eklendi. Toplam birim: {units.Count}");
    }

    /// <summary>
    /// Aşiret için yeni bir birim üret
    /// </summary>
    public Unit ProduceUnit(UnitType unitType, int x, int y)
    {
        // Birim maliyeti kontrol et
        Unit newUnit = null;
        int cost = 0;

        switch (unitType)
        {
            case UnitType.Warrior:
                newUnit = new GameObject($"Warrior_{tribeId}").AddComponent<Warrior>();
                cost = 50;
                break;
            case UnitType.Archer:
                newUnit = new GameObject($"Archer_{tribeId}").AddComponent<Archer>();
                cost = 60;
                break;
            case UnitType.Cavalry:
                newUnit = new GameObject($"Cavalry_{tribeId}").AddComponent<Cavalry>();
                cost = 75;
                break;
            case UnitType.Defender:
                newUnit = new GameObject($"Defender_{tribeId}").AddComponent<Defender>();
                cost = 70;
                break;
            case UnitType.Mage:
                newUnit = new GameObject($"Mage_{tribeId}").AddComponent<Mage>();
                cost = 80;
                break;
        }

        // Yeterli kaynakları kontrol et
        if (gold < cost)
        {
            Debug.Log($"[Tribe] {tribeName} - Yeterli altın yok! Gerekli: {cost}, Mevcut: {gold}");
            Destroy(newUnit.gameObject);
            return null;
        }

        // Birim üret
        newUnit.Initialize(x, y, tribeId, grid);
        AddUnit(newUnit);
        gold -= cost;

        Debug.Log($"[Tribe] {tribeName} - {unitType} üretildi! Kalan altın: {gold}");
        return newUnit;
    }

    /// <summary>
    /// Aşirettin sahip olduğu kareyi ekle
    /// </summary>
    public void ClaimTile(Tile tile)
    {
        if (!ownedTiles.Contains(tile))
        {
            ownedTiles.Add(tile);
            tile.SetOwner(tribeId);
        }
    }

    /// <summary>
    /// Her turu başlatırken kaynaklar ekle (ekonomi)
    /// </summary>
    public void GenerateResources()
    {
        // Her sahip olunan kare başına 1 altın ver
        int goldPerTile = ownedTiles.Count;
        gold += goldPerTile;

        // Kabile özel bonuslar
        switch (tribeType)
        {
            case TribeType.DesertTribe:
                gold += 10; // Çöl aşireti ekonomi bonusu
                break;
            case TribeType.ForestTribe:
                diamonds += 2; // Orman aşireti elmas bonusu
                break;
        }

        Debug.Log($"[Tribe] {tribeName} - Kaynaklar oluşturuldu! Altın: {gold}, Elmas: {diamonds}");
    }

    /// <summary>
    /// Tüm birimlerin tur eylemlerini sıfırla
    /// </summary>
    public void ResetUnitsActions()
    {
        foreach (var unit in units)
        {
            unit.ResetTurnActions();
        }
        Debug.Log($"[Tribe] {tribeName} - Tüm birimler ısıldı.");
    }

    /// <summary>
    /// Ölü birimleri listeden çıkar
    /// </summary>
    public void RemoveDeadUnits()
    {
        units.RemoveAll(u => u == null);
    }

    // Getter metodları
    public int GetGold() => gold;
    public int GetDiamonds() => diamonds;
    public int GetPopulation() => population;
    public int GetMaxPopulation() => maxPopulation;
    public List<Unit> GetUnits() => units;
    public int GetUnitCount() => units.Count;
    public List<Tile> GetOwnedTiles() => ownedTiles;

    // Setter metodları
    public void AddGold(int amount) => gold += amount;
    public void AddDiamonds(int amount) => diamonds += amount;
    public void AddPopulation(int amount) => population = Mathf.Min(population + amount, maxPopulation);
}

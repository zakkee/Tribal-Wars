using UnityEngine;

/// <summary>
/// Askeri birim sınıfı. Tüm birim türlerinin temel sınıfıdır.
/// </summary>
public class Unit : MonoBehaviour
{
    [SerializeField] protected UnitType unitType;
    [SerializeField] protected int tribeId;
    [SerializeField] protected int health = 100;
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int attack = 10;
    [SerializeField] protected int defense = 5;
    [SerializeField] protected int movementRange = 3;
    [SerializeField] protected int attackRange = 1;
    [SerializeField] protected int cost = 50; // Birim üretim maliyeti

    protected int currentX;
    protected int currentY;
    protected Grid grid;
    protected bool hasMovedThisTurn = false;
    protected bool hasAttackedThisTurn = false;

    public virtual void Initialize(int x, int y, int tribe, Grid gameGrid)
    {
        currentX = x;
        currentY = y;
        tribeId = tribe;
        grid = gameGrid;
        health = maxHealth;
        hasMovedThisTurn = false;
        hasAttackedThisTurn = false;

        Debug.Log($"[Unit] {unitType} başlatıldı. Konumu: ({x}, {y})");
    }

    /// <summary>
    /// Birimi harita üzerinde hareket ettir
    /// </summary>
    public virtual bool Move(int targetX, int targetY)
    {
        if (hasMovedThisTurn)
        {
            Debug.Log("[Unit] Bu tur zaten hareket ettiniz!");
            return false;
        }

        // Mesafe kontrol
        int distance = grid.GetDistance(currentX, currentY, targetX, targetY);
        if (distance > movementRange)
        {
            Debug.Log($"[Unit] Çok uzak! Hareket mesafesi: {movementRange}");
            return false;
        }

        // Hedef kareyi kontrol et
        Tile targetTile = grid.GetTile(targetX, targetY);
        if (targetTile == null || !targetTile.CanPlace(this))
        {
            Debug.Log("[Unit] Bu karaya konulamaz!");
            return false;
        }

        // Eski konumdan birim çıkart
        Tile currentTile = grid.GetTile(currentX, currentY);
        if (currentTile != null)
        {
            currentTile.OccupyingUnit = null;
        }

        // Yeni konuma hareket et
        currentX = targetX;
        currentY = targetY;
        targetTile.OccupyingUnit = this;
        hasMovedThisTurn = true;

        transform.position = targetTile.WorldPosition;

        Debug.Log($"[Unit] {unitType} ({currentX}, {currentY}) konumuna hareket etti.");
        return true;
    }

    /// <summary>
    /// Başka bir birimi saldır
    /// </summary>
    public virtual bool Attack(Unit targetUnit)
    {
        if (hasAttackedThisTurn)
        {
            Debug.Log("[Unit] Bu tur zaten saldırdınız!");
            return false;
        }

        if (targetUnit == null || targetUnit.tribeId == this.tribeId)
        {
            Debug.Log("[Unit] Geçersiz hedef!");
            return false;
        }

        // Mesafe kontrol
        int distance = grid.GetDistance(currentX, currentY, targetUnit.currentX, targetUnit.currentY);
        if (distance > attackRange)
        {
            Debug.Log($"[Unit] Hedef saldırı mesafesinin dışında! Saldırı mesafesi: {attackRange}");
            return false;
        }

        // Hasar hesapla
        int damage = CalculateDamage(targetUnit);
        targetUnit.TakeDamage(damage);
        hasAttackedThisTurn = true;

        Debug.Log($"[Unit] {unitType} saldırdı! Hasar: {damage}");
        return true;
    }

    /// <summary>
    /// Hasar hesapla (saldırı ve savunma farkı)
    /// </summary>
    protected virtual int CalculateDamage(Unit targetUnit)
    {
        int baseDamage = attack;
        int damageReduction = targetUnit.defense / 2;
        int finalDamage = Mathf.Max(1, baseDamage - damageReduction);

        // Rastgele hasar varyasyonu
        finalDamage += Random.Range(-2, 3);

        return finalDamage;
    }

    /// <summary>
    /// Hasar al
    /// </summary>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"[Unit] {unitType} {damage} hasar aldı. Kalan can: {health}/{maxHealth}");

        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Ölüm durumu
    /// </summary>
    public virtual void Die()
    {
        Debug.Log($"[Unit] {unitType} öldü!");

        // Harita karesinden çıkart
        Tile currentTile = grid.GetTile(currentX, currentY);
        if (currentTile != null)
        {
            currentTile.OccupyingUnit = null;
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Tur bitiminde durumları sıfırla
    /// </summary>
    public virtual void ResetTurnActions()
    {
        hasMovedThisTurn = false;
        hasAttackedThisTurn = false;
    }

    // Getter metodları
    public UnitType GetUnitType() => unitType;
    public int GetTribeId() => tribeId;
    public int GetHealth() => health;
    public int GetMaxHealth() => maxHealth;
    public int GetAttack() => attack;
    public int GetDefense() => defense;
    public int GetMovementRange() => movementRange;
    public int GetAttackRange() => attackRange;
    public int GetCost() => cost;
    public int GetX() => currentX;
    public int GetY() => currentY;
    public bool HasMovedThisTurn() => hasMovedThisTurn;
    public bool HasAttackedThisTurn() => hasAttackedThisTurn;
}

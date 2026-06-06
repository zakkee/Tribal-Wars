using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tur sistemi - Oyunun sirayla hareket etmesini sağlar
/// </summary>
public class TurnManager : MonoBehaviour
{
    private List<Tribe> tribes = new List<Tribe>();
    private int currentTurnIndex = 0;
    private int turnNumber = 1;
    private Tribe currentTribe;
    private bool turnInProgress = false;

    public void Initialize(List<Tribe> playingTribes)
    {
        tribes = playingTribes;
        if (tribes.Count > 0)
        {
            currentTribe = tribes[0];
            Debug.Log($"[TurnManager] Oyun başladı! İlk oyuncu: {currentTribe.tribeName}");
        }
    }

    /// <summary>
    /// Sonuç turunun başlangıcını başlat
    /// </summary>
    public void StartTurn()
    {
        if (currentTribe == null)
            return;

        turnInProgress = true;

        // Kaynaklar oluştur
        currentTribe.GenerateResources();

        // Birim eylemlerini sıfırla
        currentTribe.ResetUnitsActions();

        // Ölü birimleri temizle
        currentTribe.RemoveDeadUnits();

        Debug.Log($"[TurnManager] Tur {turnNumber} - {currentTribe.tribeName}'in sırası! (Oyuncu {currentTurnIndex + 1}/{tribes.Count})");
    }

    /// <summary>
    /// Mevcut tur bitti, sonraki oyuncuya geç
    /// </summary>
    public void EndTurn()
    {
        if (!turnInProgress)
            return;

        turnInProgress = false;

        // Sonraki oyuncu
        currentTurnIndex = (currentTurnIndex + 1) % tribes.Count;

        if (currentTurnIndex == 0)
        {
            // Yeni tur başladı
            turnNumber++;
            Debug.Log($"[TurnManager] YENİ TUR BAŞLADI! Tur numarası: {turnNumber}");
        }

        currentTribe = tribes[currentTurnIndex];
        Debug.Log($"[TurnManager] {currentTribe.tribeName}'e sıra geçti!");

        StartTurn();
    }

    /// <summary>
    /// Aşiret öldü mü kontrol et
    /// </summary>
    public bool IsTribeAlive(Tribe tribe)
    {
        return tribe.GetUnitCount() > 0;
    }

    /// <summary>
    /// Oyun bitti mi kontrol et
    /// </summary>
    public bool IsGameOver()
    {
        int aliveTribes = 0;
        foreach (var tribe in tribes)
        {
            if (IsTribeAlive(tribe))
                aliveTribes++;
        }

        return aliveTribes <= 1;
    }

    /// <summary>
    /// Kazananağı bul
    /// </summary>
    public Tribe GetWinner()
    {
        foreach (var tribe in tribes)
        {
            if (IsTribeAlive(tribe))
                return tribe;
        }
        return null;
    }

    // Getter metodları
    public Tribe GetCurrentTribe() => currentTribe;
    public int GetTurnNumber() => turnNumber;
    public int GetCurrentTribeTurn() => currentTurnIndex + 1;
    public int GetTotalTribes() => tribes.Count;
    public bool IsTurnInProgress() => turnInProgress;
    public List<Tribe> GetAllTribes() => tribes;
}

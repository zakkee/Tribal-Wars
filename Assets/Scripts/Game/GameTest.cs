using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Oyun testi ve test sahnesini yönetir
/// </summary>
public class GameTest : MonoBehaviour
{
    [SerializeField] private int mapWidth = 15;
    [SerializeField] private int mapHeight = 15;
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private BotAI.Difficulty botDifficulty = BotAI.Difficulty.Normal;

    private GameManager gameManager;
    private Grid grid;
    private TurnManager turnManager;
    private Tribe playerTribe;
    private Tribe botTribe;
    private BotAI botAI;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // GameManager'ı başlat
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            GameObject gmObject = new GameObject("GameManager");
            gameManager = gmObject.AddComponent<GameManager>();
        }

        // Harita oluştur
        grid = new Grid(mapWidth, mapHeight, tileSize);
        MapGenerator.GenerateMap(grid);

        // TurnManager oluştur
        GameObject tmObject = new GameObject("TurnManager");
        turnManager = tmObject.AddComponent<TurnManager>();

        // Oyuncu aşireti oluştur
        playerTribe = CreateTribe(0, "Player Tribe", TribeType.ForestTribe, Color.green);

        // Bot aşireti oluştur
        botTribe = CreateTribe(1, "Bot Tribe", TribeType.MountainTribe, Color.red);

        // TurnManager'ı başlat
        List<Tribe> tribes = new List<Tribe> { playerTribe, botTribe };
        turnManager.Initialize(tribes);

        // Bot AI'yi oluştur
        GameObject botAIObject = new GameObject("BotAI");
        botAI = botAIObject.AddComponent<BotAI>();
        botAI.Initialize(botTribe, grid, turnManager, botDifficulty);

        // Test birimlerini oluştur
        CreateTestUnits();

        // İlk turu başlat
        turnManager.StartTurn();

        Debug.Log("[GameTest] Oyun başlatıldı! Harita: " + mapWidth + "x" + mapHeight);
    }

    /// <summary>
    /// Aşiret oluştur
    /// </summary>
    private Tribe CreateTribe(int id, string name, TribeType type, Color color)
    {
        GameObject tribeObject = new GameObject(name);
        Tribe tribe = tribeObject.AddComponent<Tribe>();
        tribe.Initialize(id, name, type, color, grid);
        TribeProperties.ApplyBonuses(tribe, grid);
        return tribe;
    }

    /// <summary>
    /// Test için başlangıç birimlerini oluştur
    /// </summary>
    private void CreateTestUnits()
    {
        // Oyuncu için test birimlerini oluştur
        playerTribe.ProduceUnit(UnitType.Warrior, 2, 2);
        playerTribe.ProduceUnit(UnitType.Archer, 3, 2);

        // Bot için test birimlerini oluştur
        botTribe.ProduceUnit(UnitType.Warrior, 12, 12);
        botTribe.ProduceUnit(UnitType.Cavalry, 11, 12);

        Debug.Log("[GameTest] Test birimler oluşturuldu!");
    }

    private void Update()
    {
        // Test kontrolleri
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Tur bitir
            turnManager.EndTurn();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            // Bot turunu oynat
            botAI.PlayTurn();
        }

        // Oyun bittiğini kontrol et
        if (turnManager.IsGameOver())
        {
            Tribe winner = turnManager.GetWinner();
            if (winner != null)
            {
                Debug.Log($"[GameTest] OYUN BİTTİ! Kazanan: {winner.tribeName}");
            }
        }
    }

    /// <summary>
    /// Şu anki oyun durumunu konsola yazdır
    /// </summary>
    public void PrintGameStatus()
    {
        Debug.Log("\n=== OYUN DURUMU ===");
        Debug.Log($"Tur: {turnManager.GetTurnNumber()}");
        Debug.Log($"Oyuncu: {turnManager.GetCurrentTribe().tribeName}");

        foreach (var tribe in turnManager.GetAllTribes())
        {
            Debug.Log($"\n{tribe.tribeName}:");
            Debug.Log($"  - Altın: {tribe.GetGold()}");
            Debug.Log($"  - Elmas: {tribe.GetDiamonds()}");
            Debug.Log($"  - Birim Sayısı: {tribe.GetUnitCount()}");
            Debug.Log($"  - Sahip Olunan Bölge: {tribe.GetOwnedTiles().Count}");
        }
    }
}

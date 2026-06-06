using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Oyunun ana yöneticisi. Oyun akışını, tur sistemini ve oyuncu kontrolünü yönetir.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField] private float tileSize = 1f;

    private Grid grid;
    private TurnManager turnManager;
    private List<Tribe> tribes = new List<Tribe>();
    private GameState gameState = GameState.Playing;

    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Harita oluştur
        grid = new Grid(mapWidth, mapHeight, tileSize);
        MapGenerator.GenerateMap(grid);

        // Tur yöneticisini başlat
        turnManager = gameObject.AddComponent<TurnManager>();
        turnManager.Initialize(tribes);

        Debug.Log($"[GameManager] Oyun başlatıldı! Harita boyutu: {mapWidth}x{mapHeight}");
    }

    public void AddTribe(Tribe tribe)
    {
        tribes.Add(tribe);
        Debug.Log($"[GameManager] {tribe.tribeName} aşireti eklendi.");
    }

    public Grid GetGrid()
    {
        return grid;
    }

    public TurnManager GetTurnManager()
    {
        return turnManager;
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
        Debug.Log($"[GameManager] Oyun durumu: {state}");
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void EndGame()
    {
        SetGameState(GameState.GameOver);
        Debug.Log("[GameManager] Oyun bitti!");
    }
}

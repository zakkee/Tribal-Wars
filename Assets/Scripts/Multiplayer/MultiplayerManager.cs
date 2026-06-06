using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Çok oyunculu oyun yöneticisi - Sunucu ve istemci tarafında çalışır
/// </summary>
public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance { get; private set; }

    private BluetoothManager bluetoothManager;
    private GameManager gameManager;
    private TurnManager turnManager;
    private bool isHost = false;
    private int localTribeId = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        bluetoothManager = BluetoothManager.Instance;
        gameManager = GameManager.Instance;
    }

    /// <summary>
    /// Çok oyunculu oyunu sunucu olarak başlat
    /// </summary>
    public void StartAsHost()
    {
        isHost = true;
        localTribeId = 0;
        bluetoothManager.StartBluetoothServer();
        Debug.Log("[MultiplayerManager] Sunucu olarak başlatıldı.");
    }

    /// <summary>
    /// Çok oyunculu oyunu istemci olarak başlat
    /// </summary>
    public void StartAsClient(string hostDevice)
    {
        isHost = false;
        localTribeId = 1;
        bluetoothManager.StartBluetoothClient(hostDevice);
        Debug.Log("[MultiplayerManager] İstemci olarak başlatıldı.");
    }

    /// <summary>
    /// Birim hareket mesajı gönder
    /// </summary>
    public void SendUnitMove(int unitId, int targetX, int targetY)
    {
        MoveMessage moveMsg = new MoveMessage
        {
            unitId = unitId,
            targetX = targetX,
            targetY = targetY
        };

        string jsonData = JsonUtility.ToJson(moveMsg);
        NetworkMessage netMsg = new NetworkMessage("MOVE", localTribeId, jsonData);
        bluetoothManager.SendMessage(netMsg);

        Debug.Log($"[MultiplayerManager] Birim hareket mesajı gönderildi: ({targetX}, {targetY})");
    }

    /// <summary>
    /// Birim saldırı mesajı gönder
    /// </summary>
    public void SendUnitAttack(int attackerId, int targetId)
    {
        AttackMessage attackMsg = new AttackMessage
        {
            attackerId = attackerId,
            targetId = targetId
        };

        string jsonData = JsonUtility.ToJson(attackMsg);
        NetworkMessage netMsg = new NetworkMessage("ATTACK", localTribeId, jsonData);
        bluetoothManager.SendMessage(netMsg);

        Debug.Log($"[MultiplayerManager] Birim saldırı mesajı gönderildi.");
    }

    /// <summary>
    /// Birim üretim mesajı gönder
    /// </summary>
    public void SendProduceUnit(UnitType unitType, int spawnX, int spawnY)
    {
        ProduceMessage produceMsg = new ProduceMessage
        {
            unitType = (int)unitType,
            spawnX = spawnX,
            spawnY = spawnY
        };

        string jsonData = JsonUtility.ToJson(produceMsg);
        NetworkMessage netMsg = new NetworkMessage("PRODUCE", localTribeId, jsonData);
        bluetoothManager.SendMessage(netMsg);

        Debug.Log($"[MultiplayerManager] Birim üretim mesajı gönderildi.");
    }

    /// <summary>
    /// Tur bitirme mesajı gönder
    /// </summary>
    public void SendEndTurn()
    {
        EndTurnMessage endMsg = new EndTurnMessage
        {
            tribeId = localTribeId,
            turnNumber = turnManager.GetTurnNumber()
        };

        string jsonData = JsonUtility.ToJson(endMsg);
        NetworkMessage netMsg = new NetworkMessage("END_TURN", localTribeId, jsonData);
        bluetoothManager.SendMessage(netMsg);

        Debug.Log($"[MultiplayerManager] Tur bitirme mesajı gönderildi.");
    }

    /// <summary>
    /// Gelen mesajları işle
    /// </summary>
    public void ProcessIncomingMessage(NetworkMessage message)
    {
        if (message == null) return;

        switch (message.messageType)
        {
            case "MOVE":
                HandleMoveMessage(message);
                break;
            case "ATTACK":
                HandleAttackMessage(message);
                break;
            case "PRODUCE":
                HandleProduceMessage(message);
                break;
            case "END_TURN":
                HandleEndTurnMessage(message);
                break;
            default:
                Debug.LogWarning($"[MultiplayerManager] Bilinmeyen mesaj türü: {message.messageType}");
                break;
        }
    }

    private void HandleMoveMessage(NetworkMessage message)
    {
        MoveMessage moveMsg = JsonUtility.FromJson<MoveMessage>(message.data);
        Debug.Log($"[MultiplayerManager] Hareket mesajı alındı: Unit {moveMsg.unitId} -> ({moveMsg.targetX}, {moveMsg.targetY})");
        // Birim hareketi oyun içinde uygulanır
    }

    private void HandleAttackMessage(NetworkMessage message)
    {
        AttackMessage attackMsg = JsonUtility.FromJson<AttackMessage>(message.data);
        Debug.Log($"[MultiplayerManager] Saldırı mesajı alındı: Saldırgan {attackMsg.attackerId} -> Hedef {attackMsg.targetId}");
        // Saldırı oyun içinde uygulanır
    }

    private void HandleProduceMessage(NetworkMessage message)
    {
        ProduceMessage produceMsg = JsonUtility.FromJson<ProduceMessage>(message.data);
        Debug.Log($"[MultiplayerManager] Üretim mesajı alındı: Birim Türü {produceMsg.unitType}");
        // Birim üretimi oyun içinde uygulanır
    }

    private void HandleEndTurnMessage(NetworkMessage message)
    {
        EndTurnMessage endMsg = JsonUtility.FromJson<EndTurnMessage>(message.data);
        Debug.Log($"[MultiplayerManager] Tur bitirme mesajı alındı: Tur {endMsg.turnNumber}");
        // Tur bitişi oyun içinde uygulanır
    }

    private void Update()
    {
        // Gelen mesajları kontrol et ve işle
        NetworkMessage incomingMessage = bluetoothManager.ReceiveMessage();
        if (incomingMessage != null)
        {
            ProcessIncomingMessage(incomingMessage);
        }
    }

    // Getter metodları
    public bool IsHost() => isHost;
    public int GetLocalTribeId() => localTribeId;
    public bool IsConnected() => bluetoothManager.IsConnected() || bluetoothManager.IsServer();
}

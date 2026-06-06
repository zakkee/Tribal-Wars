using System;

/// <summary>
/// Ağ üzerinde iletişim için mesaj yapısı
/// </summary>
[Serializable]
public class NetworkMessage
{
    public string messageType; // "MOVE", "ATTACK", "PRODUCE", "END_TURN", vb.
    public int senderId;
    public string data; // JSON formatında veri
    public long timestamp;

    public NetworkMessage(string type, int sender, string messageData)
    {
        messageType = type;
        senderId = sender;
        data = messageData;
        timestamp = System.DateTime.Now.Ticks;
    }
}

/// <summary>
/// Birim hareket mesajı
/// </summary>
[Serializable]
public class MoveMessage
{
    public int unitId;
    public int targetX;
    public int targetY;
}

/// <summary>
/// Birim saldırı mesajı
/// </summary>
[Serializable]
public class AttackMessage
{
    public int attackerId;
    public int targetId;
}

/// <summary>
/// Birim üretim mesajı
/// </summary>
[Serializable]
public class ProduceMessage
{
    public int unitType; // UnitType enum değeri
    public int spawnX;
    public int spawnY;
}

/// <summary>
/// Tur bitirme mesajı
/// </summary>
[Serializable]
public class EndTurnMessage
{
    public int tribeId;
    public int turnNumber;
}

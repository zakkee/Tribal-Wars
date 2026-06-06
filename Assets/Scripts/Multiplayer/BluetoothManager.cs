using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Bluetooth üzerinden çok oyunculu bağlantısını yönetir
/// </summary>
public class BluetoothManager : MonoBehaviour
{
    public static BluetoothManager Instance { get; private set; }

    private string deviceName = "Tribal-Wars";
    private string connectedDeviceName = "";
    private bool isConnected = false;
    private bool isServer = false;
    private Queue<string> messageQueue = new Queue<string>();

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

    /// <summary>
    /// Bluetooth'u başlat ve sunucu olarak dinlemeye başla
    /// </summary>
    public void StartBluetoothServer()
    {
        Debug.Log("[BluetoothManager] Bluetooth sunucu başlatılıyor...");
        isServer = true;
        isConnected = false;

        // Android native Bluetooth kodunu burada çağırabilirsiniz
        // Şimdilik simülasyon için false değeri kullanıyoruz
        Debug.Log($"[BluetoothManager] Sunucu {deviceName} adıyla dinlemeye başladı.");
    }

    /// <summary>
    /// Bluetooth'u başlat ve istemci olarak cihazlara bağlan
    /// </summary>
    public void StartBluetoothClient(string targetDevice)
    {
        Debug.Log($"[BluetoothManager] {targetDevice} cihazına bağlanılıyor...");
        isServer = false;
        connectedDeviceName = targetDevice;

        // Android native Bluetooth kodunu burada çağırabilirsiniz
        Debug.Log($"[BluetoothManager] {targetDevice} cihazına bağlantı kuruldu.");
        isConnected = true;
    }

    /// <summary>
    /// Mesaj gönder
    /// </summary>
    public void SendMessage(NetworkMessage message)
    {
        if (!isConnected && !isServer)
        {
            Debug.LogWarning("[BluetoothManager] Bağlantı kurulmamış!");
            return;
        }

        string jsonMessage = JsonUtility.ToJson(message);
        Debug.Log($"[BluetoothManager] Mesaj gönderiliyor: {jsonMessage}");

        // Android native Bluetooth kodunu burada çağırabilirsiniz
        // Bluetooth üzerinden veri gönderme
    }

    /// <summary>
    /// Mesaj al
    /// </summary>
    public NetworkMessage ReceiveMessage()
    {
        if (messageQueue.Count > 0)
        {
            string jsonMessage = messageQueue.Dequeue();
            NetworkMessage message = JsonUtility.FromJson<NetworkMessage>(jsonMessage);
            return message;
        }
        return null;
    }

    /// <summary>
    /// Mesajı mesaj kuyruğuna ekle
    /// </summary>
    public void EnqueueMessage(string jsonMessage)
    {
        messageQueue.Enqueue(jsonMessage);
    }

    /// <summary>
    /// Bluetooth bağlantısını kapat
    /// </summary>
    public void Disconnect()
    {
        Debug.Log("[BluetoothManager] Bluetooth bağlantısı kapatılıyor...");
        isConnected = false;
        isServer = false;
        messageQueue.Clear();
    }

    /// <summary>
    /// Bağlı cihazlar listesini al
    /// </summary>
    public List<string> GetPairedDevices()
    {
        List<string> devices = new List<string>
        {
            "Device1",
            "Device2",
            "Device3"
        };
        Debug.Log("[BluetoothManager] Eşleştirilmiş cihazlar alındı.");
        return devices;
    }

    // Getter metodları
    public bool IsConnected() => isConnected;
    public bool IsServer() => isServer;
    public string GetConnectedDeviceName() => connectedDeviceName;
    public string GetDeviceName() => deviceName;
}

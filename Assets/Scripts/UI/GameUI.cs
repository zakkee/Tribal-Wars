using UnityEngine;
using TMPro;

/// <summary>
/// Oyun UI'ı - Ekonomi, tur bilgisi ve kontroller
/// </summary>
public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tribeNameText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI diamondsText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI unitsCountText;
    [SerializeField] private TextMeshProUGUI tilesOwnedText;

    private TurnManager turnManager;
    private GameManager gameManager;

    public void Initialize(TurnManager turnsManager, GameManager gamesManager)
    {
        turnManager = turnsManager;
        gameManager = gamesManager;
        CreateUICanvas();
    }

    private void CreateUICanvas()
    {
        // Canvas oluştur
        GameObject canvasObj = new GameObject("GameUICanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // UI elemanları oluştur
        CreateUIElements(canvasObj.transform);
    }

    private void CreateUIElements(Transform canvasParent)
    {
        // Tribe Name
        tribeNameText = CreateTextElement("TribeName", canvasParent, "Tribe: Player Tribe", new Vector2(10, -30), new Vector2(300, 50));
        
        // Gold
        goldText = CreateTextElement("Gold", canvasParent, "Gold: 500", new Vector2(10, -80), new Vector2(300, 50));
        
        // Diamonds
        diamondsText = CreateTextElement("Diamonds", canvasParent, "Diamonds: 100", new Vector2(10, -130), new Vector2(300, 50));
        
        // Turn
        turnText = CreateTextElement("Turn", canvasParent, "Turn: 1", new Vector2(10, -180), new Vector2(300, 50));
        
        // Units Count
        unitsCountText = CreateTextElement("UnitsCount", canvasParent, "Units: 0", new Vector2(10, -230), new Vector2(300, 50));
        
        // Tiles Owned
        tilesOwnedText = CreateTextElement("TilesOwned", canvasParent, "Tiles: 0", new Vector2(10, -280), new Vector2(300, 50));
    }

    private TextMeshProUGUI CreateTextElement(string name, Transform parent, string initialText, Vector2 position, Vector2 size)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.parent = parent;
        textObj.transform.localPosition = Vector3.zero;

        RectTransform rectTransform = textObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;

        TextMeshProUGUI textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = initialText;
        textComponent.fontSize = 36;
        textComponent.color = Color.white;

        return textComponent;
    }

    public void UpdateUI(Tribe currentTribe)
    {
        if (currentTribe == null) return;

        tribeNameText.text = $"Tribe: {currentTribe.tribeName}";
        goldText.text = $"Gold: {currentTribe.GetGold()}";
        diamondsText.text = $"Diamonds: {currentTribe.GetDiamonds()}";
        turnText.text = $"Turn: {turnManager.GetTurnNumber()}";
        unitsCountText.text = $"Units: {currentTribe.GetUnitCount()}";
        tilesOwnedText.text = $"Tiles: {currentTribe.GetOwnedTiles().Count}";
    }
}

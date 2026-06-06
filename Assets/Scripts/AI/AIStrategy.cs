/// <summary>
/// AI stratejileri tanımlar
/// </summary>
public class AIStrategy
{
    private BotAI.Difficulty difficulty;

    public AIStrategy(BotAI.Difficulty diff)
    {
        difficulty = diff;
    }

    /// <summary>
    /// Kolay zorluk: Basit hamle yap
    /// </summary>
    public void PlayEasyMode()
    {
        // Rastgele hamle
    }

    /// <summary>
    /// Normal zorluk: Dengeli strateji
    /// </summary>
    public void PlayNormalMode()
    {
        // Keşfet ve saldır dengesi
    }

    /// <summary>
    /// Zor zorluk: Agresif strateji
    /// </summary>
    public void PlayHardMode()
    {
        // Düşmanları hedefle ve yok et
    }

    public BotAI.Difficulty GetDifficulty() => difficulty;
}

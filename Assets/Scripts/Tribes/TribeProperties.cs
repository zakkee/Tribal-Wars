/// <summary>
/// Aşiret özellikleri ve bonusları tanımlar
/// </summary>
public static class TribeProperties
{
    public static void ApplyBonuses(Tribe tribe, Grid grid)
    {
        switch (tribe.tribeType)
        {
            case TribeType.ForestTribe:
                ApplyForestBonuses(tribe);
                break;
            case TribeType.MountainTribe:
                ApplyMountainBonuses(tribe);
                break;
            case TribeType.WaterTribe:
                ApplyWaterBonuses(tribe);
                break;
            case TribeType.DesertTribe:
                ApplyDesertBonuses(tribe);
                break;
            case TribeType.SnowTribe:
                ApplySnowBonuses(tribe);
                break;
            case TribeType.SwampTribe:
                ApplySwampBonuses(tribe);
                break;
        }
    }

    private static void ApplyForestBonuses(Tribe tribe)
    {
        // Orman aşireti: Ormanda hızlı hareket, birim maliyeti az
        tribe.AddGold(50);
        Debug.Log($"[TribeProperties] {tribe.tribeName} - Orman bonusu uygulandı!");
    }

    private static void ApplyMountainBonuses(Tribe tribe)
    {
        // Dağ aşireti: Güçlü savunma, yüksek can
        tribe.AddGold(30);
        Debug.Log($"[TribeProperties] {tribe.tribeName} - Dağ bonusu uygulandı!");
    }

    private static void ApplyWaterBonuses(Tribe tribe)
    {
        // Su aşireti: Suda hızlı hareket, keşif bonusu
        tribe.AddGold(40);
        Debug.Log($"[TribeProperties] {tribe.tribeName} - Su bonusu uygulandı!");
    }

    private static void ApplyDesertBonuses(Tribe tribe)
    {
        // Çöl aşireti: Ekonomi bonusu, daha fazla kaynak
        tribe.AddGold(80);
        tribe.AddDiamonds(20);
        Debug.Log($"[TribeProperties] {tribe.tribeName} - Çöl bonusu uygulandı!");
    }

    private static void ApplySnowBonuses(Tribe tribe)
    {
        // Kar aşireti: Kar bölgesinde güçlü, teknoloji bonusu
        tribe.AddGold(35);
        Debug.Log($"[TribeProperties] {tribe.tribeName} - Kar bonusu uygulandı!");
    }

    private static void ApplySwampBonuses(Tribe tribe)
    {
        // Bataklık aşireti: Gizli birimler, tuzak
        tribe.AddGold(45);
        Debug.Log($"[TribeProperties] {tribe.tribeName} - Bataklık bonusu uygulandı!");
    }
}

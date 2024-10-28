

public enum GameState
{
    MENU,
    WEAPONSELECTION,
    GAME,
    GAMEOVER,
    STAGECOMPLETE,
    WAVETRANSITION,
    SHOP
}

public enum Stat
{
    Attack,
    AttackSpeed,
    CriticalChance,
    CriticalPercent,
    MoveSpeed,
    MaxHealth,
    Range,
    HealthRecoverySpeed,
    Armor,
    Luck,
    Dodge,
    LifeSteal
}

public static class Enums
{
    public static string FormatStatName(Stat stat)
    {
        string formatted = "";
        string unFormatted = stat.ToString();

        if (unFormatted.Length <= 0)
            return "UnValid Stat Name";
        
        formatted += unFormatted[0];
        
        for (int i = 1; i < unFormatted.Length; i++)
        {
            if (char.IsUpper(unFormatted[i]))
            {
                formatted += " ";
            }
            
            formatted += unFormatted[i];
        }
        
        return formatted;
    }
}
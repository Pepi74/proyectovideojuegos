[System.Serializable]
public class Upgrade
{
    public string name;
    public UpgradeType type;
    public int value;
    public float rarity;

    public Upgrade(string name, UpgradeType type, int value, float rarity)
    {
        this.name = name;
        this.type = type;
        this.value = value;
        this.rarity = rarity;
    }
}

public enum UpgradeType
{
    LevelIncrease,
    MaxHpIncrease,
    MaxStaminaIncrease,
    StaminaRegenIncrease,
    AttackDamageIncrease
}
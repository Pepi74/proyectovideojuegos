[System.Serializable]
public class Upgrade
{
    public string name;
    public string description;
    public UpgradeType type;
    public int value;
    public float rarity;

    public Upgrade(string name, string description, UpgradeType type, int value, float rarity)
    {
        this.name = name;
        this.description = description;
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
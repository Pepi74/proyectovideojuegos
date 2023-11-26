using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public Upgrade[] upgrades;
    public Button[] upgradeButtons;

    private int upgradePoints;

    private void Start()
    {
        // Randomize and display initial upgrades
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        upgrades = new[]
        {
            new Upgrade("Increase Level by 1", UpgradeType.LevelIncrease, 1, 5),
            new Upgrade("Increase Max HP by 10", UpgradeType.MaxHpIncrease, 10, 25),
            new Upgrade("Increase Max Stamina by 5", UpgradeType.MaxStaminaIncrease, 5, 25),
            new Upgrade("Increase Stamina regeneration by 1", UpgradeType.StaminaRegenIncrease, 1, 15),
            new Upgrade("Increase attack damage by 1", UpgradeType.AttackDamageIncrease, 1, 15),
            
            // Rare variants
            new Upgrade("Rare - Increase Level by 2", UpgradeType.LevelIncrease, 2, 0.5f),
            new Upgrade("Rare - Increase Max HP by 20", UpgradeType.MaxHpIncrease, 20, 1),
            new Upgrade("Rare - Increase Max Stamina by 10", UpgradeType.MaxStaminaIncrease, 10, 1),
            new Upgrade("Rare - Increase Stamina regeneration by 2", UpgradeType.StaminaRegenIncrease, 2, 1),
            new Upgrade("Rare - Increase attack damage by 2", UpgradeType.AttackDamageIncrease, 2, 1),

        };
    }

    public void RandomizeUpgrades()
    {
        List<Upgrade> availableUpgrades = new List<Upgrade>(upgrades);

        // Select at least four unique upgrades without repeats
        List<Upgrade> randomizedUpgrades = new List<Upgrade>();
        while (randomizedUpgrades.Count < Mathf.Min(4, availableUpgrades.Count))
        {
            int randomIndex = Random.Range(0, availableUpgrades.Count);
            Upgrade selectedUpgrade = availableUpgrades[randomIndex];

            // Check if the selected upgrade type is already in the list
            bool alreadySelected = randomizedUpgrades.Exists(upgrade => upgrade.type == selectedUpgrade.type);

            // Check if the upgrade should be included based on its upgrade rarity
            if (alreadySelected || !(Random.Range(0f, 100f) <= selectedUpgrade.rarity)) continue;
            randomizedUpgrades.Add(selectedUpgrade);
            availableUpgrades.RemoveAt(randomIndex);
        }

        // Shuffle the list of upgrades
        for (int i = randomizedUpgrades.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (randomizedUpgrades[i], randomizedUpgrades[randomIndex]) =
                (randomizedUpgrades[randomIndex], randomizedUpgrades[i]);
        }

        DisplayUpgrades(randomizedUpgrades);
    }

    private void DisplayUpgrades(IReadOnlyList<Upgrade> randomizedUpgrades)
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i >= randomizedUpgrades.Count) continue;
            upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = randomizedUpgrades[i].name;
            UpgradeButton upgradeButton = upgradeButtons[i].GetComponent<UpgradeButton>();
            upgradeButton.upgrade = randomizedUpgrades[i];
        }
    }
}

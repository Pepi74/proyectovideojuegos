using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Upgrade upgrade;
    public PlayerScript playerScript;

    private void Start()
    {
        // Attach the button click event listener
        Button button = GetComponent<Button>();
        button.onClick.AddListener(HandleUpgradeButtonClick);
    }
    
    private void Awake()
    {
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.onPlayerSpawned.AddListener(SetPlayerReference);
    }

    private void HandleUpgradeButtonClick()
    {
        // Call the ApplyUpgrade method in PlayerScript with the selected upgrade
        playerScript.ApplyUpgrade(upgrade);
    }
    
    private void SetPlayerReference(GameObject spawnedPlayer)
    {
        playerScript = spawnedPlayer.GetComponent<PlayerScript>();
    }
}
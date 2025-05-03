using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private GameObject freezeEffectPrefab;
    [SerializeField] private GameObject luckyDipEffectPrefab;
    [SerializeField] private GameObject invincibleEffectPrefab;

    [SerializeField] private PlayerController player;
    [SerializeField] private InventorySlotUI[] slots;

    private Dictionary<string, int> inventory = new();
    private Dictionary<string, PowerUpCommand> commands = new();

    public void Awake()
    {
        if (Instance == null) Instance = this;

        inventory["Freeze"] = 0;
        inventory["Invincibility"] = 0;
        inventory["LuckyDip"] = 0;

        commands["Freeze"] = new FreezePowerUp(7, freezeEffectPrefab);
        commands["Invincibility"] = new InvincibilityPowerUp(invincibleEffectPrefab);
        commands["LuckyDip"] = new LuckyDipPowerUp(luckyDipEffectPrefab);

        UpdateUI();
    }

    public void AddPowerUp(string name, int amount = 1)
    {
        if (inventory.ContainsKey(name))
        {
            inventory[name] += amount;
            UpdateUI();
        }
    }

    public void UsePowerUp(string name, PlayerController player)
    {
        if (inventory.ContainsKey(name) && inventory[name] > 0)
        {
            commands[name].Execute(player);
            inventory[name]--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        int i = 0;
        foreach (var pair in inventory)
        {
            slots[i].UpdateSlot(pair.Value);
            i++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UsePowerUp("Freeze", player);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UsePowerUp("Invincibility", player);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UsePowerUp("LuckyDip", player);
        }
    }

    public void UseFreeze()
    {
        UsePowerUp("Freeze", player);

    }

    public void UseInvincibility()
    {
        UsePowerUp("Invincibility", player);
    }

    public void UseLuckyDip()
    {
        UsePowerUp("LuckyDip", player);
    }


}

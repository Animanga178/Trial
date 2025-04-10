using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private InventorySlotUI[] slots;

    private Dictionary<string, int> inventory = new();
    private Dictionary<string, PowerUpCommand> commands = new();
}

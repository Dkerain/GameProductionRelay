using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [Header("CURRENCY")]
    [SerializeField] private int _currency = 0;

    [Header("ITEMS")]
    [SerializeField] private List<InventoryItem> _items = new List<InventoryItem>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 获取当前货币数量
    public int GetCurrency() => _currency;

    // 添加货币
    public void AddCurrency(int amount)
    {
        _currency += amount;
        Debug.Log($"获得货币: +{amount} (当前: {_currency})");
    }

    // 消耗货币
    public bool SpendCurrency(int amount)
    {
        if (_currency < amount) return false;

        _currency -= amount;
        Debug.Log($"消耗货币: -{amount} (当前: {_currency})");
        return true;
    }

    // 添加物品
    public void AddItem(string itemId, int quantity = 1)
    {
        var existingItem = _items.Find(i => i.id == itemId);

        if (existingItem != null)
        {
            existingItem.quantity += quantity;
        }
        else
        {
            _items.Add(new InventoryItem(itemId, quantity));
        }

        Debug.Log($"获得物品: {itemId} x{quantity}");
    }

    // 检查物品数量
    public int GetItemCount(string itemId)
    {
        var item = _items.Find(i => i.id == itemId);
        return item?.quantity ?? 0;
    }

    // 移除物品
    public bool RemoveItem(string itemId, int quantity = 1)
    {
        var item = _items.Find(i => i.id == itemId);

        if (item == null || item.quantity < quantity) return false;

        item.quantity -= quantity;

        if (item.quantity <= 0)
            _items.Remove(item);

        Debug.Log($"移除物品: {itemId} x{quantity}");
        return true;
    }

    // 库存物品结构
    [System.Serializable]
    public class InventoryItem
    {
        public string id;
        public int quantity;

        public InventoryItem(string id, int quantity)
        {
            this.id = id;
            this.quantity = quantity;
        }
    }
}
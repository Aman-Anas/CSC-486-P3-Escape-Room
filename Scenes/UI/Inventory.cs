using Godot;
using System.Collections.Generic;

public partial class Inventory : Control
{
    
    private class InventoryItem
    {
        public int count { get; set; }
        public int index { get; set; }
    }
    
    private Dictionary<Item, InventoryItem> _lookup = new();
    private List<InventoryItem> _contents = new();
    
    public void ClearInventory()
    {
        _contents.Clear();
    }
    
    public void AddItem(Item item)
    {
        if (item == null) return;
        if (_lookup.TryGetValue(item, out var inventoryItem)) inventoryItem.count++;
        else {
            var newItem = new InventoryItem { count = 1, index = _contents.Count };
            _contents.Add(newItem);
            _lookup[item] = newItem;
        }
    }
    
    public void RemoveItem(Item item)
    {
        if (item == null) return;
        if (!_lookup.TryGetValue(item, out var inventoryItem)) return;
        
        inventoryItem.count--;
        if (inventoryItem.count <= 0) {
            _lookup.Remove(item);
            _contents.Remove(inventoryItem);
            
            for (int i = 0; i < _contents.Count; i++) {
                _contents[i].index = i;
            }
        }
    }
    
    public bool HasItem(Item item)
    {
        return item != null && _lookup.ContainsKey(item);
    }
}

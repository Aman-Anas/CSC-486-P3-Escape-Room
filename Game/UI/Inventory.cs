using Godot;
using System.Collections.Generic;

public partial class Inventory : Control
{
    
    private class InventoryItem
    {
        public Item item { get; set; } = null!;
        public int count { get; set; }
        public int index { get; set; }
        public HotbarSlot slot { get; set; } = null!;
    }
    
    private Dictionary<Item, InventoryItem> _lookup = new();
    private List<InventoryItem> _contents = new();
    //private Dictionary<InventoryItem, HotbarSlot> _slotLookup = new();
    
    [Export] public PackedScene slotScene = null!;
    [Export] public HBoxContainer hotbarContainer = null!;
    
    // clear just inventory items
    public void ClearInventory()
    {
        _contents.Clear();
    }
    
    //private void _GenerateSlotLookup()
    //{
        //
        //_slotLookup.Clear();
    //}
    
    // clear and regenerate hotbar slots
    public void RefreshHotbar()
    {
        foreach (Node child in hotbarContainer.GetChildren())
        {
            child.QueueFree();
        }
        
        foreach (var inventoryItem in _contents)
        {
            inventoryItem.slot?.QueueFree();
            var slot = slotScene.Instantiate<HotbarSlot>();
            inventoryItem.slot = slot;
            slot.SetItem(inventoryItem.item);
            hotbarContainer.AddChild(slot);
        }
    }
    
    public override void _Ready()
    {
        RefreshHotbar();
    }
    
    public void AddItem(Item item)
    {
        if (item == null) return;
        if (_lookup.TryGetValue(item, out var inventoryItem)) {
            inventoryItem.count++;
            
            // update count in hotbar
            // with keys and statues, this probably won't ever be called
            // since there's only ever a single key or statue of a type, i think
        }
        else {
            HotbarSlot slot = slotScene.Instantiate<HotbarSlot>();
            
            var newItem = new InventoryItem { count = 1, index = _contents.Count, item = item, slot = slot };
            _contents.Add(newItem);
            _lookup[item] = newItem;
            
            // add icon to hotbar
            slot.SetItem(item);
            //GD.Print(slot);
            hotbarContainer.AddChild(slot);
        }
        
        GD.Print($"[Inventory] - Picked up: {item.Name}");
    }
    
    public void RemoveItem(Item item)
    {
        if (item == null) return;
        if (!_lookup.TryGetValue(item, out var inventoryItem)) return;
        
        inventoryItem.count--;
        if (inventoryItem.count <= 0) {
            _lookup.Remove(item);
            _contents.Remove(inventoryItem);
            inventoryItem.slot.QueueFree();
            
            for (int i = 0; i < _contents.Count; i++) {
                _contents[i].index = i;
            }
        }
    }
    
    public void RemoveItemByResource(Resource resource)
    {
        if (resource == null) return;
        if (resource is Item item)
        {
            RemoveItem(item);
        }
    }
    
    public bool HasItem(Item item)
    {
        return item != null && _lookup.ContainsKey(item);
    }
    
    public bool HasItemByResource(Resource resource)
    {
        return resource != null && resource is Item item && _lookup.ContainsKey(item);
    }
}

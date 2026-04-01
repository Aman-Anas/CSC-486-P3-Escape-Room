using Godot;
using System;

public partial class HotbarSlot : Control
{
    [Export]
    public TextureRect icon = null!;
    
    public void SetItem(Item item)
    {
        icon.Texture = item?.Icon;
    }
}

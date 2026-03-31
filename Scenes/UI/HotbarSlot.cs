using Godot;
using System;

public partial class HotbarSlot : Control
{
    public override void _Ready()
    {
        GD.Print("Slot initialized at: " + GlobalPosition);
        Visible = true; 
    }
    
    [Export]
    Control root = null!;
    
    public void SetSize(int size)
    {
        root.CustomMinimumSize = new Vector2(size, size);
        root.Size = new Vector2(size, size);
    }
}

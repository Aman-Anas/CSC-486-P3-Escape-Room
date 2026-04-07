using Godot;
using System;

[GlobalClass]
public partial class RuneResource : Resource
{
    [Export] public string Name { get; private set; } = "";
    
    [Export] public Color RuneColor { get; set; } = Color.FromHsv(1, 1, 1);

    [Export] public Texture2D? Icon { get; private set; }
}

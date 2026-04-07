using Godot;
using System;

public partial class CipherPuzzle : Node3D
{
    [Export] public CipherPuzzleLayer[] Layers = [];

    private int[] _key = [];

    public bool Check()
    {
        for (int i = 0; i < Layers.Length; i++)
        {
            if (Layers[i].SelectionIndex != _key[i]) return false;
        }

        foreach (CipherPuzzleLayer layer in Layers) layer.Activate();

        return true;
    }

    private void PrintKey()
    {
        GD.Print($"cipher key: {string.Join(", ", _key)}");
    }

    public override void _Ready()
    {
        int numRunes = Layers[0].RuneTextures.Length;
        int numLayers = Layers.Length;

        // generate key
        _key = new int[numLayers];
        for (int i = 0; i < numLayers; i++) _key[i] = CipherPuzzleLayer.random.Next(numRunes);
        PrintKey();

        // randomize selection
        foreach (CipherPuzzleLayer layer in Layers) layer.RandomizeSelection();
    }
}

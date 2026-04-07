using Godot;
using System;

public partial class CipherPuzzle : Node3D
{
    [Export] public CipherPuzzleLayer[] Layers = [];
    [Export] public MeshInstance3D FrontMarker = null!;
    [Export] public Color SolvedColor = new("#ffff00");
    [Export] public Color UnsolvedColor = new("#d2241e");

    private int[] _key = [];

    public bool Check()
    {
        for (int i = 0; i < Layers.Length; i++)
        {
            if (Layers[i].SelectionIndex != _key[i]) return false;
        }

        // layer solved color
        foreach (CipherPuzzleLayer layer in Layers) layer.Activate();

        // front marker solved color
        if (FrontMarker.GetActiveMaterial(0) is StandardMaterial3D material)
        {
            material.EmissionEnabled = true;
            material.AlbedoColor = new("#777777");
        }

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

        // front marker unsolved color
        if (FrontMarker.GetActiveMaterial(0) is StandardMaterial3D material)
        {
            material.AlbedoColor = UnsolvedColor;
            material.EmissionEnabled = false;
            material.Emission = SolvedColor;
        }
    }
}

using Godot;
using System;
// using System.Collections.Generic;

public partial class CipherPuzzleLayer : Node3D
{
    [Export] public CompressedTexture2D[] RuneTextures = [];
    [Export] public Color RuneColor = new("#7f543d");

    private static readonly Random random = new();

    private int _rotationIndex = 0;
    public int SelectionIndex { get; private set; } = 0;
    private double _runeDistance = 1.01;

    public int RandomizeSelection()
    {
        SelectionIndex = random.Next(RuneTextures.Length);
        _rotationIndex = SelectionIndex;
        DoRotation();
        return SelectionIndex;
    }

    private void ClampSelected()
    {
        SelectionIndex = (SelectionIndex + RuneTextures.Length) % RuneTextures.Length;
    }

    private void PrintSelected()
    {
        GD.Print($"selected: {SelectionIndex}");
    }

    private void DoRotation()
    {
        Tween tween = CreateTween();
        float target = (float)_rotationIndex / RuneTextures.Length * 2.0f * (float)Math.PI;
        tween.TweenProperty(this, "rotation:y", target, 0.5f)
            .SetTrans(Tween.TransitionType.Quart)
            .SetEase(Tween.EaseType.Out);
    }

    public void RotateLeft()
    {
        _rotationIndex--;
        DoRotation();

        SelectionIndex--;
        ClampSelected();
        PrintSelected();
    }

    public void RotateRight()
    {
        _rotationIndex++;
        DoRotation();

        SelectionIndex++;
        ClampSelected();
        PrintSelected();
    }

    public override void _Ready()
    {
        // create runes around edge
        for (int i = 0; i < RuneTextures.Length; i++)
        {
            Sprite3D sprite = new Sprite3D();
            sprite.Texture = RuneTextures[i];
            sprite.Modulate = RuneColor;

            // set position
            double angle = (double)i / RuneTextures.Length * 2 * Math.PI;
            sprite.Position = new Vector3(
                (float)Math.Sin(angle),
                0,
                (float)Math.Cos(angle)
            );

            // set rotation
            sprite.Rotation = sprite.Rotation with
            {
                Y = (float)angle
            };

            // set scale
            sprite.Scale *= (float)0.28;

            AddChild(sprite);
        }
    }
}

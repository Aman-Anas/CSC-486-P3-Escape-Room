using Godot;
using System;

public partial class CipherPuzzleLayer : Node3D
{
    [Export] public CompressedTexture2D[] RuneTextures = [];
    [Export] public Color SelectedRuneColor = new("#ffff00");
    [Export] public Color InactiveRuneColor = new("#000000");
    [Export] public Color ActivatedRuneColor = new("#ffff00");
    [Export] public float TweenTime = 0.5f;

    public static readonly Random random = new();
    private bool AllowRotation = true;

    private int _rotationIndex = 0;
    public int SelectionIndex { get; private set; } = 0;
    private double _runeDistance = 1.01;

    public int RandomizeSelection()
    {
        SelectionIndex = random.Next(RuneTextures.Length);
        _rotationIndex = SelectionIndex;
        DoRotation();
        ActivateRune(SelectionIndex);
        return SelectionIndex;
    }

    public void ActivateRune(int index)
    {
        SetRuneColor(index, SelectedRuneColor);
    }

    public void DeactivateRune(int index)
    {
        SetRuneColor(index, InactiveRuneColor);
    }

    public void CorrectRune()
    {
        SetRuneColor(SelectionIndex, ActivatedRuneColor);
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
        float target = -(float)_rotationIndex / RuneTextures.Length * 2.0f * (float)Math.PI;
        tween.TweenProperty(this, "rotation:y", target, TweenTime)
            .SetTrans(Tween.TransitionType.Quart)
            .SetEase(Tween.EaseType.Out);
    }

    public void RotateLeft()
    {
        _rotationIndex++;
        DoRotation();

        DeactivateRune(SelectionIndex);
        SelectionIndex++;
        ClampSelected();
        ActivateRune(SelectionIndex);
        PrintSelected();
    }

    public void RotateRight()
    {
        _rotationIndex--;
        DoRotation();

        DeactivateRune(SelectionIndex);
        SelectionIndex--;
        ClampSelected();
        ActivateRune(SelectionIndex);
        PrintSelected();
    }

    private Sprite3D[] _runes = [];

    public void SetRuneColor(int index, Color color)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(_runes[index], "modulate", color, TweenTime)
            .SetTrans(Tween.TransitionType.Quart)
            .SetEase(Tween.EaseType.Out);
    }

    public override void _Ready()
    {
        // create runes around edge
        _runes = new Sprite3D[RuneTextures.Length];
        for (int i = 0; i < RuneTextures.Length; i++)
        {
            Sprite3D rune = new Sprite3D();
            _runes[i] = rune;
            rune.Texture = RuneTextures[i];
            rune.Modulate = InactiveRuneColor;

            // set position
            double angle = (double)i / RuneTextures.Length * 2 * Math.PI;
            rune.Position = new Vector3(
                (float)Math.Sin(angle),
                0,
                (float)Math.Cos(angle)
            );

            // set rotation
            rune.Rotation = rune.Rotation with
            {
                Y = (float)angle
            };

            // set scale
            rune.Scale *= (float)0.28;

            AddChild(rune);
        }
    }
}

using Godot;
using System;
using System.Collections.Generic;

public partial class CipherPuzzleLayer : Node3D
{
    [Export] public CompressedTexture2D[] RuneTextures = [];
    [Export] public Color RuneColor = new Color("#7f543d");
    
    private int rotationIndex = 0;
    private int selected = 0;
    private double runeDistance = 1.01;
    
    private void _ClampSelected()
    {
        selected = (selected + RuneTextures.Length) % RuneTextures.Length;
    }
    
    private void _PrintSelected()
    {
        GD.Print($"selected: {selected}");
    }
    
    private void _DoRotation()
    {
        Tween tween = CreateTween();
        float target = (float)rotationIndex / RuneTextures.Length * 2.0f * (float)Math.PI;
        tween.TweenProperty(this, "rotation:y", target, 0.5f)
            .SetTrans(Tween.TransitionType.Quart)
            .SetEase(Tween.EaseType.Out);
    }
    
    public void RotateLeft()
    {
        rotationIndex--;
        _DoRotation();
        
        selected--;
        _ClampSelected();
        _PrintSelected();
    }
    
    public void RotateRight()
    {
        rotationIndex++;
        _DoRotation();
            
        selected++;
        _ClampSelected();
        _PrintSelected();
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
            sprite.Rotation = sprite.Rotation with {
                Y = (float)angle
            };
            
            // set scale
            sprite.Scale *= (float)0.28;
            
            AddChild(sprite);
        }
    }
}

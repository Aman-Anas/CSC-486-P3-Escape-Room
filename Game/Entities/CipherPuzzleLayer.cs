using Godot;
using System;
using System.Collections.Generic;

public partial class CipherPuzzleLayer : Node3D
{
    [Export] public CompressedTexture2D[] RuneTextures = [];
    [Export] public Color RuneColor = new Color("#7f543d");
    
    private int selected = 0;
    private double runeDistance = 1.01;
    
    private void _ClampSelected()
    {
        selected %= RuneTextures.Length;
    }
    
    public void RotateLeft()
    {
        selected++; // don't clamp now, rotation needs to wrap around length
        
        Tween tween = CreateTween();
        float target = (float)selected / RuneTextures.Length * 2.0f * (float)Math.PI;
        tween.TweenProperty(this, "rotation:y", target, 0.5f)
            .SetTrans(Tween.TransitionType.Quart)
            .SetEase(Tween.EaseType.Out);
        
        _ClampSelected();
    }
    
    public void RotateRight()
    {
        
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

using Godot;

namespace Game.Entities;

[GlobalClass]
public partial class LaserMirror : Node3D, IPlayerInteractable
{
    [Export]
    public LaserPuzzleController Controller { get; set; } = null!;

    [Export]
    public CollisionShape3D CollisionShape { get; set; } = null!;

    [Export]
    public Color InteractionColor { get; set; } = new(0.65f, 0.9f, 1f, 1f);

    [Export]
    public float PlacementYawOffsetDegrees { get; set; } = 90f;

    public LaserMirrorSocket? CurrentSocket { get; private set; }

    public bool IsCarried { get; private set; }

    public override void _Ready()
    {
        SetCarriedState(false);
    }

    public string GetInteractionText(Farmer farmer)
    {
        if (IsCarried)
        {
            return string.Empty;
        }

        if (Controller.IsCarryingMirror(farmer))
        {
            return "Already carrying a mirror block.";
        }

        return "[e] pick up mirror block";
    }

    public Color GetInteractionColor()
    {
        return InteractionColor;
    }

    public void Interact(Farmer farmer)
    {
        if (IsCarried)
        {
            return;
        }

        Controller.TryPickupMirror(farmer, this);
    }

    public void SetCarriedState(bool carried)
    {
        IsCarried = carried;

        Visible = !carried;

        if (IsInstanceValid(CollisionShape))
        {
            CollisionShape.Disabled = carried;
        }
    }

    public void PlaceOnSocket(LaserMirrorSocket socket)
    {
        CurrentSocket = socket;

        var snapTransform = socket.GetSnapTransform();
        var yawOffsetRadians = Mathf.DegToRad(PlacementYawOffsetDegrees);
        var rotatedBasis = snapTransform.Basis.Rotated(Vector3.Up, yawOffsetRadians);

        GlobalTransform = new Transform3D(rotatedBasis, snapTransform.Origin);
        SetCarriedState(false);
    }

    public void ClearSocketReference()
    {
        CurrentSocket = null;
    }
}

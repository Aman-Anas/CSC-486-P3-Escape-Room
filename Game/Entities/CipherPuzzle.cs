using Godot;
using System;

public partial class CipherPuzzle : Node3D
{
    // static cipher key
    // instance of ciphertext
    // method for checking validity of cipher
    // get list of children
    [Export] public CipherPuzzleLayer[] Layers = [];
}

using Godot;
using System;

public partial class TestCharacterMove : Sprite2D
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        var vx = GD.RandRange(-50, 50);
        var vy = GD.RandRange(-50, 50);
        var value = new Vector2(vx, vy);
        Position += new Vector2((float)(value.X * delta), (float)(value.Y * delta));
    }

}

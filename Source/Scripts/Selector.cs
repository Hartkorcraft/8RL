using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;


public class Selector : Sprite
{
    [Export] public float MoveSpeed { get; set; } = 0.3f;

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (this.Visible)
        {
            Position = LerpTo(Position);
        }
    }

    private Vector2 LerpTo(Vector2 pos)
    {
        return Lerp(pos, (Battlescape.WorldToMap(GetGlobalMousePosition()) * Battlescape.TILE_SIZE).Vec2() - new Vector2(1, 1), MoveSpeed);
    }
}

using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;


public abstract class MapObject : Node2D, INameable, IHealthSystem
{
    public static MapObject? CurrentSelection = null;
    
    public virtual string ObjectName { get; protected set; } = "Map_Object";
    public IHealth HealthSystem { get; protected set; } = new HealthSystem();

    protected Vector2i gridPos;
    public virtual Vector2i GridPos
    {
        get => gridPos;
        set
        {
            Position = value.Vec2() * new Vector2(Battlescape.TILE_SIZE, Battlescape.TILE_SIZE);
            gridPos = value;
        }
    }

    public override void _EnterTree()
    {
        GridPos = GetGridPosFromPosition(Position);
    }

    public static Vector2i GetGridPosFromPosition(Vector2 pos)
    {
        return new Vector2i(pos / new Vector2(Battlescape.TILE_SIZE, Battlescape.TILE_SIZE));
    }
}

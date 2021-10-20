using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;


public class Battlescape : Node2D
{
    public const int TILE_SIZE = 16;

    Map? map;
    public static MouseModule? MouseModule { get; private set; }
    public static SelectionModule? SelectionModule { get; private set; }
    public static Vector2i WorldToMap(Vector2 pos) => new Vector2i(((int)(pos.x / Battlescape.TILE_SIZE)), ((int)(pos.y / Battlescape.TILE_SIZE)));
    public static World2D? World2D { get; private set; }
    public override void _EnterTree()
    {
        World2D = GetWorld2d();
        MouseModule = (MouseModule)GetNode("MouseModule");
        SelectionModule = (SelectionModule)GetNode("SelectionModule");
        map = new Map(
       new Vector2i(50, 50),
       (TileMap)GetNode("Tiles/Floor"),
       (TileMap)GetNode("Tiles/Mid"),
       (TileMap)GetNode("Tiles/Pathfinding"),
       (TileMap)GetNode("Tiles/PossiblePositions"),
       clearMap: false
        );
    }

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {

    }
}

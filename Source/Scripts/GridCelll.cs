using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class GridCell
{
    public readonly Vector2i GridPos;

    private readonly Map map;
    private TileType floorTile;
    private TileType midTile;

    public TileType FloorTile
    {
        get => floorTile;
        set
        {
            floorTile = value;
            Map.SetTile(GridPos, floorTile, Map.FloorTilesKey);
        }
    }
    public TileType MidTile
    {
        get => midTile;
        set
        {
            midTile = value;
            Map.SetTile(GridPos, midTile, Map.MidTilesKey);
        }
    }

    public GridCell(Map _map, Vector2i _gridPos, TileType _floorTile = TileType.Grass, TileType _midTile = TileType.Empty)
    {
        map = _map;
        GridPos = _gridPos;
        FloorTile = _floorTile;
        MidTile = _midTile;
    }
}

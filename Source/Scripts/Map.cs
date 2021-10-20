using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class Map
{
    GridCell[,] gridMap;

    static Dictionary<string, TileMap?> tileMaps = new Dictionary<string, TileMap?>();
    public const string FloorTilesKey = "FloorTiles";
    public const string MidTilesKey = "MidTiles";
    public const string PathfindingTilesKey = "PathfindingTiles";
    public const string PossiblePositionsTilesKey = "PossiblePositionsTiles";

    public static PathFinding<TileType>? PathFinding { get; private set; }

    public static void SetTile(Vector2i pos, TileType tileType, string layerKey)
    {
        tileMaps[layerKey]?.SetCellv(pos.Vec2(), ((int)tileType));
    }

    public static void ClearTilemap(string layerKey)
    {
        tileMaps[layerKey]?.Clear();
    }

    public static (TileType floorTile, TileType midTile, TileType pathTIle) GetTilesFromPos(Vector2i pos)
    {
        return
        (((TileType?)tileMaps[FloorTilesKey]?.GetCellv(pos.Vec2())) ?? TileType.Empty,
        ((TileType?)tileMaps[MidTilesKey]?.GetCellv(pos.Vec2())) ?? TileType.Empty,
        ((TileType?)tileMaps[PathfindingTilesKey]?.GetCellv(pos.Vec2())) ?? TileType.Empty);
    }

    public static List<T> GetFromGridPos<T>(Vector2i _pos, uint mask = 2147483647)
    {
        var colliders = GetColliderDictsFromGridPos(_pos, mask);
        var found = new List<T>();

        foreach (var collider in colliders)
        {
            if (collider["collider"] is Area2D && ((Area2D)collider["collider"]).GetParent() is T)
            {
                var objectFound = (T)(object)((Area2D)collider["collider"]).GetParent(); //Godot bug forces casting 
                found.Add(objectFound);
            }
        }
        return found;
    }

    public static bool CheckForCollisionOnGridPos(Vector2i _pos, uint mask = 2147483647)
    {
        var pos = (_pos.Vec2() * Battlescape.TILE_SIZE) + new Vector2((float)Battlescape.TILE_SIZE / 2, (float)Battlescape.TILE_SIZE / 2);

        Godot.Collections.Array result = Battlescape.World2D?.DirectSpaceState.IntersectPoint(pos, 32, null, mask, true, true) ?? new Godot.Collections.Array();
        return result.Count > 0;
    }

    private static List<Godot.Collections.Dictionary> GetColliderDictsFromGridPos(Vector2i _pos, uint mask = 2147483647)
    {
        var pos = (_pos.Vec2() * Battlescape.TILE_SIZE) + new Vector2((float)Battlescape.TILE_SIZE / 2, (float)Battlescape.TILE_SIZE / 2);

        Godot.Collections.Array result = Battlescape.World2D?.DirectSpaceState.IntersectPoint(pos, 32, null, mask, true, true) ?? new Godot.Collections.Array();

        var list = new List<Godot.Collections.Dictionary>();
        foreach (Godot.Collections.Dictionary collider_dict in result)
        {
            list.Add(collider_dict);
        }
        return list;
    }

    public Map(Vector2i _mapSize, TileMap f, TileMap m, TileMap _pathfindingTiles, TileMap posiblePossitions, bool clearMap = true)
    {
        gridMap = new GridCell[_mapSize.x, _mapSize.y];

        tileMaps[FloorTilesKey] = f;
        tileMaps[MidTilesKey] = m;
        tileMaps[PathfindingTilesKey] = _pathfindingTiles;
        tileMaps[PossiblePositionsTilesKey] = posiblePossitions;

        Func<Vector2i, HashSet<TileType>, bool> checkForBlocking =
        (pos, blocking) =>
        {
            return blocking.Contains((TileType)tileMaps[MidTilesKey]!.GetCellv(pos.Vec2()));
        };

        Func<Vector2i, uint, bool> check_for_colliders =
        (pos, mask) => { return GetColliderDictsFromGridPos(pos, mask).Count > 0; };

        PathFinding = new PathFinding<TileType>(_mapSize, checkForBlocking, check_for_colliders);

        for (int y = 0; y < _mapSize.y; y++)
        {
            for (int x = 0; x < _mapSize.x; x++)
            {
                var tiles = GetTilesFromPos(new Vector2i(x, y));
                if (clearMap) { gridMap[x, y] = new GridCell(this, new Vector2i(x, y), TileType.Grass, TileType.Empty); }
                else { gridMap[x, y] = new GridCell(this, new Vector2i(x, y), tiles.floorTile, tiles.midTile); }
            }
        }
    }
}

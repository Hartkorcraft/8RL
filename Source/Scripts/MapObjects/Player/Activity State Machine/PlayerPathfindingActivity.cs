using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerPathfindingActivity : PlayerActivityBase
{
    public List<Vector2i> pathPositionsCache { get; protected set; } = new List<Vector2i>();
    bool EmptyPathCache { get => pathPositionsCache.Count == 0; }

    public override void DoPlayerBeingSelectedProcess()
    {
        //Pathfinding
        if (playerCharacter.TurnSystem.MovementPoints > 0 && (MouseModule.DiffrentLastMousePos || EmptyPathCache))
        {
            if (Map.PathFinding is not null && playerCharacter.area2D is not null)
            {

                var path = Map.PathFinding.FindPath(
                    startPos: playerCharacter.GridPos,
                    endPos: MouseModule.MouseGridPos,
                    blockingTiles: playerCharacter.blockingMovement,
                    collider_mask: playerCharacter.area2D.CollisionMask,
                    diagonals: true,
                    big: false);

                if (PathNotEmpty(path) && PossiblePositionsContainPath(path!))
                {
                    DrawingPath(path!);
                }
            }
        }

        bool PathNotEmpty(List<PathFindingCell<TileType>>? path) => (path != null && path.Count > 0);
        bool PossiblePositionsContainPath(List<PathFindingCell<TileType>> path) => possiblePositionsCache.Contains(path[path.Count - 1].GridPos);
    }

    private void DrawingPath(List<PathFindingCell<TileType>> path)
    {
        pathPositionsCache.Clear();
        Map.ClearTilemap(Map.PathfindingTilesKey);
        var distance = 0;
        foreach (var pathTile in path)
        {
            if (distance < playerCharacter.TurnSystem.MovementPoints)
            {
                Map.SetTile(pathTile.GridPos, TileType.Red_Dot, Map.PathfindingTilesKey);
                pathPositionsCache.Add(pathTile.GridPos);
            }
            else
            {
                Map.SetTile(pathTile.GridPos, TileType.Green_Dot, Map.PathfindingTilesKey);
            }
            distance++;
        }
    }

    public override void PlayerInput(InputEvent inputEvent)
    {
        //*Movement
        if (inputEvent.IsActionPressed("Left_Mouse"))
        {
            if (EmptyPathCache == false && PathLastPositionMatchMouse())
            {
                playerCharacter.TurnSystem.MovementPoints -= pathPositionsCache.Count;
                playerCharacter.MoveOnPath(pathPositionsCache);
                pathPositionsCache.Clear();
            }
        }

        bool PathLastPositionMatchMouse() => pathPositionsCache[pathPositionsCache.Count - 1].Equals(MouseModule.MouseGridPos);
    }

    public override void UpdateCalculations()
    {
        CalculatePossiblePositions();
        foreach (var point in possiblePositionsCache)
        {
            Map.SetTile(point, TileType.Green_Dot, Map.PossiblePositionsTilesKey);
        }
    }

    public PlayerPathfindingActivity(PlayerCharacter p) : base(p) { }
}


using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class PlayerActivityBase
{
    protected PlayerCharacter playerCharacter;

    public virtual void Start() { GD.Print($"Changed activity to {this.GetType()}"); }
    public virtual void Exit() { }
    public virtual void PlayerInput(InputEvent _input) { }
    public virtual void DoPlayerActionProcess() { }
    public virtual void DoPlayerBeingSelectedProcess() { }
    public virtual void UpdateCalculations() { }
    public List<Vector2i> possiblePositionsCache { get; set; } = new List<Vector2i>();
    public virtual void ClearPossiblePositionsCache() { possiblePositionsCache.Clear(); }
    public virtual void CalculatePossiblePositions()
    {
        possiblePositionsCache = Map.PathFinding?.FindPossibleSpaces(
            playerCharacter.GridPos,
            playerCharacter.TurnSystem.MovementPoints,
            playerCharacter.blockingMovement,
            playerCharacter.area2D!.CollisionMask) ?? new List<Vector2i>();
        GD.Print(playerCharacter.GridPos, playerCharacter.TurnSystem.MovementPoints, playerCharacter.blockingMovement);
    }

    public PlayerActivityBase(PlayerCharacter _p)
    {
        playerCharacter = _p;
        ActivitySystem.updateActivity += UpdateCalculations;
    }
}

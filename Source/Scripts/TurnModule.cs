using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HartLib;
using static HartLib.Utils;

public class TurnModule
{
    public HashSet<ITurnable> PlayerTurnObjects { get; } = new HashSet<ITurnable>();
    public HashSet<ITurnable> NpcTurnObjects { get; } = new HashSet<ITurnable>();

    //TODO refactor
    public void StartPlayerTurn()
    {
        foreach (ITurnable playerObject in PlayerTurnObjects) { playerObject.StartTurn(); }
        foreach (ITurnable npcObject in NpcTurnObjects) { npcObject.EndTurn(); }
        UpdateTurnObjects();
    }

    public void StartNpcTurn()
    {
        foreach (ITurnable npcObject in NpcTurnObjects) { npcObject.StartTurn(); }
        foreach (ITurnable playerObject in PlayerTurnObjects) { playerObject.EndTurn(); }
        UpdateTurnObjects();
    }

    public void UpdateTurnObjects()
    {
        foreach (ITurnable npcObject in NpcTurnObjects)
        {
            npcObject.UpdateTurnObject();
        }
        foreach (ITurnable playerObject in PlayerTurnObjects)
        {
            playerObject.UpdateTurnObject();
        }
    }

    public void NextTurn()
    {

    }
}

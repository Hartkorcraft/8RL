using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HartLib;
using static HartLib.Utils;

public class StateModule
{
    public GameState CurrentState { get; private set; }
    public GameState? PreviousState { get; private set; }
    public static event Action<GameState>? ChangedStateEvent;
    public static StartState StartState { get; private set; } = new StartState();
    public static PlayerTurnState PlayerTurnState { get; private set; } = new PlayerTurnState();
    public static PlayerUseState PlayerUseState { get; private set; } = new PlayerUseState();
    public static TransitionState TransitionState { get; private set; } = new TransitionState();
    public static NpcTurnState NpcTurnState { get; private set; } = new NpcTurnState();

    public void SetState(GameState? newGameState, bool init = true)
    {
        if (newGameState is null) { return; }
        if (newGameState == CurrentState) { return; }
        if (CurrentState is not null) { CurrentState.ExitState(); }
        PreviousState = CurrentState;
        CurrentState = newGameState;
        if (init) { CurrentState.Init(); }
        ChangedStateEvent?.Invoke(newGameState);
        CurrentState.ReadyState();
        DebugManager.UpdateLog("GameStates", "Current: " + CurrentState?.GetType().ToString() + " Previous: " + PreviousState?.GetType().ToString());
    }

    public void SetPreviousState() => SetState(PreviousState, init: false);
    public void UpdateCurrentState() => CurrentState.UpdateState();
    public bool CurrentStateStateCanSelect { get => CurrentState.CanSelect == true; }
    public bool CurrentStateAllowWorldInput { get => CurrentState.AllowWorldInput == true; }

    public void EndTransitionState()
    {
        if (CurrentState is TransitionState)
        {
            SetPreviousState();
        }
        else { GD.PrintErr("not transitioning!"); }
    }

    public StateModule()
    {
        CurrentState = StartState;
    }
}

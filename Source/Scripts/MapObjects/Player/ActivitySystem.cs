using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class ActivitySystem
{
    public static event Action<PlayerCharacter>? changedActivity;
    public static event Action? updateActivity;
    public PlayerActivityBase DefaultActivity { get; private set; }
    public PlayerActivityBase? PreviousActivity { get; private set; } = null;

    public PlayerNonTurnActivity NonTurnActivity { get; protected set; }
    public PlayerIdleActivity IdleActivity { get; protected set; }
    public PlayerPathfindingActivity PathfindingActivity { get; protected set; }

    private PlayerActivityBase currentActivity;
    private PlayerCharacter player;

    public PlayerActivityBase CurrentActivity
    {
        get => currentActivity;
        private set
        {
            if (currentActivity == value) { return; }
            PreviousActivity = currentActivity;
            PreviousActivity?.Exit();
            currentActivity = value;
            currentActivity.Start();
            currentActivity.UpdateCalculations();
            changedActivity?.Invoke(player);
        }
    }

    public void SetActivity(PlayerActivityBase activity) { CurrentActivity = activity; }
    public PlayerActivityBase ChangeToDefaultActivity() => CurrentActivity = DefaultActivity;
    public PlayerActivityBase ChangeToPreviousActivity() => CurrentActivity = PreviousActivity ??= IdleActivity;
    public void DoActivityProcess() { CurrentActivity?.DoPlayerActionProcess(); }
    public void DoPlayerBeingSelectedProcess()
    {
        if (Battlescape.SelectionModule?.CurrentSelection == player) { CurrentActivity?.DoPlayerBeingSelectedProcess(); }
    }
    public void DoActivityInput(InputEvent inputEvent) { CurrentActivity?.PlayerInput(inputEvent); }
    public void UpdateCalculations() => currentActivity.UpdateCalculations();

    public void EndTransition() { }

    public ActivitySystem(PlayerCharacter _player)
    {
        player = _player;

        IdleActivity = new PlayerIdleActivity(player);
        NonTurnActivity = new PlayerNonTurnActivity(player);
        PathfindingActivity = new PlayerPathfindingActivity(player);

        DefaultActivity = IdleActivity;
        currentActivity = IdleActivity;
        CurrentActivity = PathfindingActivity;
    }
}

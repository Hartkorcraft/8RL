using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerCharacter : Entity, IGetInfoable, IInteractable, ISelectable
{
    public static int numberOfPlayerCharacters { get; private set; } = 0;
    public static event Action<PlayerCharacter>? changedActivity;

    public bool CanSelect { get; private set; }
    public override string ObjectName { get; protected set; } = "Player";
    public ActivitySystem? ActivitySystem { get; private set; }


    #region ENTER_TREE READY ETC
    public override void _EnterTree()
    {
        base._EnterTree();
        numberOfPlayerCharacters++;

        ActivitySystem = new ActivitySystem(this);
        TurnSystem = new PlayerTurnSystem(this);
        HealthSystem = new PlayerHealthSystem(this);

        ObjectName += $"_{numberOfPlayerCharacters}";
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        numberOfPlayerCharacters--;
    }

    public override void _Ready()
    {
        base._Ready();
        //ActivitySystem.ChangeToDefaultActivity();
        GD.Print("Spawned at: " + Position);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        ActivitySystem?.DoActivityProcess();
        HandleBeingSelectedProcess();
    }

    public override void _Input(InputEvent inputEvent)
    {

    }

    #endregion

    protected override void EndTransition()
    {
        base.EndTransition();
        ActivitySystem?.EndTransition();
    }


    public override void MouseEntered()
    {
        OnMouseEnter();
        DebugManager.UpdateLog("Map_Object_under_mouse",
        $"\n   {Battlescape.MouseModule?.GetMouseOverHashSetString() ?? ""} \n         {GetInfo()}",
         display: true, displayIfEmpty: false);
    }

    public override void MouseExited()
    {
        OnMouseExit();
        DebugManager.UpdateLog("Map_Object_under_mouse",
        $"\n   {Battlescape.MouseModule?.GetMouseOverHashSetString() ?? ""}",
         display: true, displayIfEmpty: false);
    }

    public override void OnLeftMouseClick()
    {
        Battlescape.SelectionModule?.Select(this, true);
    }

    #region ISelectable
    public override void HandleSelection()
    {
        //ActivitySystem?.ChangeToDefaultActivity();
    }

    public override void HandleUnselection()
    {
        //ActivitySystem?.SetActivity(ActivitySystem.IdleActivity);
    }

    public override void HandleBeingSelectedProcess()
    {
        ActivitySystem?.DoPlayerBeingSelectedProcess();
    }
    #endregion

    #region IInteractable
    public void Interact(Entity entity)
    {
        GD.Print("Interacted with ", Helpers.NameAndType(this));
    }
    #endregion

    #region IGetInfoable
    public string GetInfo()
    {
        return
        $@"*Name: {ObjectName}
        -Current_Action: {ActivitySystem?.CurrentActivity?.ToString() ?? ""}
        -GridPos: {GridPos.ToString()}
        -Movement points and Cap: {TurnSystem.MovementPoints}/{TurnSystem.MovementPointsCap}";
        //-Health and Cap: {HealthSystem.Health}/{HealthSystem.HealthCap}
    }

    #endregion

}
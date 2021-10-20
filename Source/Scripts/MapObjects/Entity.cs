using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class Entity : SpriteMapObject, ITurnSystem //, ISelectable
{
    public override string ObjectName { get; protected set; } = "Entity";

    public ITurnable TurnSystem { get; protected set; } = new TurnSystem();

     public override void _EnterTree() { base._EnterTree(); }
     public override void _ExitTree() { base._ExitTree(); }

    public virtual void HandleSelection() { }
    public virtual void HandleBeingSelectedProcess() { }
    public virtual void HandleUnselection() { }
}

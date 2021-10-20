using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class GameState
{
    public virtual bool CanSelect { get; set; } = true;

    protected bool allowWorldInput = true;
    protected bool allowActivityChange = true;

    public virtual bool AllowActivityChange
    {
        get { return allowActivityChange; }
        set => allowActivityChange = value;
    }
    public virtual bool AllowWorldInput
    {
        get { return allowWorldInput; }
        set => allowWorldInput = value;
    }

    public abstract void Init();

    #region READY UPDATE EXIT 
    public virtual void ReadyState()
    {
        GD.Print("Entered " + GetType().ToString() + " state");
    }
    public virtual void UpdateState() {  }
    public virtual void ExitState() { }
    #endregion
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HartLib;
using static HartLib.Utils;

public class SelectionModule : Node2D
{
    private ISelectable? currentSelection;
    public ISelectable? CurrentSelection
    {
        get { return currentSelection; }
        set
        {
            CurrentSelection?.HandleUnselection();
            currentSelection = value;
            CurrentSelection?.HandleSelection();

            string name = "null";
            if (currentSelection is INameable) { name = Helpers.NameAndType((INameable)currentSelection); }

            var Iname = (currentSelection as INameable)?.ObjectName;
            DebugManager.UpdateLog("Current_Selection", $"{name} {Iname}");
        }
    }

    public override void _EnterTree()
    {
        DebugManager.AddLog(new DebugInfo("Current_Selection"));
    }

    public void Select(ISelectable? selection, bool unSelectIfSame = false)
    {
        //if (StateModule.CurrentStateStateCanSelect is false) { return; }
        if (CurrentSelection == selection && unSelectIfSame) { selection = null; }
        CurrentSelection = selection;
    }


}

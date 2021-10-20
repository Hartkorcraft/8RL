using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HartLib;
using static HartLib.Utils;

public class MouseModule : Node2D
{
    public static Vector2i LastClickedGridPos { get; private set; }
    public static Vector2i LastGridPos { get; private set; }
    public static Vector2i MouseGridPos { get; private set; }
    public static bool DiffrentLastMousePos { get => !LastGridPos.Equals(MouseGridPos); }

    public HashSet<IMouseable> Mouseover { get; private set; } = new HashSet<IMouseable>();

    public void AddMouseOverObject(IMouseable imouseable) { Mouseover.Add(imouseable); }
    public void RemoveMouseOverObject(IMouseable imouseable) { Mouseover.Remove(imouseable); }

    public override void _Process(float delta)
    {
        var newMouseGridPos = Battlescape.WorldToMap(GetGlobalMousePosition());
        if (newMouseGridPos != MouseGridPos)
        {
            LastGridPos = MouseGridPos;
            MouseGridPos = newMouseGridPos;
        }
    }

    public string GetMouseOverHashSetString()
    {
        string imouseableObjects = "";
        if (Mouseover.Count == 0) return "";
        foreach (var imouseable in Mouseover)
        {
            imouseableObjects += imouseable.ToString() + " " + imouseable.GetType().Name + ", ";
        }
        return imouseableObjects;
    }

    public MouseModule()
    {
        SpriteMapObject.mouseEnterOverEvent += AddMouseOverObject;
        SpriteMapObject.mouseExitOverEvent += RemoveMouseOverObject;

    }
}

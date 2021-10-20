using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public interface IInteractable
{
    Vector2i GridPos { get; }
    bool MouseOver { get; }
    void Interact(Entity entity);
}

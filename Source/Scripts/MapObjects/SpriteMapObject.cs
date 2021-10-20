using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;


public class SpriteMapObject : MapObject, IMouseable
{
    public override string ObjectName { get; protected set; } = "Sprite_Map_Object";

    public static event Action<SpriteMapObject>? endTransitionEvent;
    public static event Action<GameState, bool>? movingOnPathEvent;
    public static event Action<SpriteMapObject>? mouseEnterOverEvent;
    public static event Action<SpriteMapObject>? mouseExitOverEvent;
    public static event Action<SpriteMapObject, InputEvent>? clickedEvent;

    public Area2D? area2D { get; private set; }
    public bool Transitioning { get; private set; } = false;
    public bool MouseOver { get; private set; } = false;
    public HashSet<TileType> blockingMovement { get; protected set; } = new HashSet<TileType>()
    {
        TileType.BlueWall,
        TileType.RedWall
    };

    [Export] private float TransitionSpeed = 0.3f;
    private List<Vector2i> transitionPositions = new List<Vector2i>(); // Positions for moving on a path

    public override void _EnterTree()
    {
        base._EnterTree();
        area2D = (Area2D)GetNode("Area2D");
    }

    public override void _ExitTree() { base._ExitTree(); }
    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(float delta)
    {
        TransitionPositionLerp(TransitionSpeed);
    }

    protected void TransitionPositionLerp(float smooth = 0.3f, float clip_range = 1f)
    {
        if (transitionPositions.Count > 0)
        {
            var new_grid_pos = (Vector2i)transitionPositions[0];
            Vector2 new_pos = new_grid_pos.Vec2() * Battlescape.TILE_SIZE;
            if (InClipRange(new_pos, clip_range))
            {
                transitionPositions.RemoveAt(0);
                GridPos = new_grid_pos;
                if (transitionPositions.Count <= 0) { EndTransition(); }
            }
            Position = Lerp(Position, new_pos, smooth);
        }

        bool InClipRange(Vector2 new_pos, float clip_range) => (Mathf.Abs(Position.x - new_pos.x) <= clip_range && Mathf.Abs(Position.y - new_pos.y) <= clip_range);
    }

    public virtual void InputEvent(Node viepoint, InputEvent inputEvent, int local_shape)
    {
        if (inputEvent is InputEventMouseButton)
        {
            if (inputEvent.IsPressed())
            {
                ClickedEventLogic(inputEvent);
                if (inputEvent.IsActionPressed("Left_Mouse"))
                {
                    OnLeftMouseClick();
                }
                else if (inputEvent.IsActionPressed("Right_Mouse"))
                {
                    OnRightMouseClick();
                }
            }
        }
    }
    public virtual void ClickedEventLogic(InputEvent input)
    {
        clickedEvent?.Invoke(this, input);
    }

    public virtual void OnLeftMouseClick() { }
    public virtual void OnRightMouseClick() { }

    public virtual void MouseEntered()
    {
        DebugManager.UpdateLog("Map_Object_under_mouse",
        $"\n   Battlescape.MouseModule.GetMouseOverHashSetString()",
         display: true, displayIfEmpty: false);
    }

    public virtual void MouseExited()
    {
        DebugManager.UpdateLog("Map_Object_under_mouse",
        $"\n   Battlescape.MouseModule.GetMouseOverHashSetString()",
         display: true, displayIfEmpty: false);
    }

    public void OnMouseEnter()
    {
        mouseEnterOverEvent?.Invoke(this);
    }

    public void OnMouseExit()
    {
        mouseExitOverEvent?.Invoke(this);
    }

    protected virtual void EndTransition()
    {
        endTransitionEvent?.Invoke(this);
    }

    public void MoveOnPath(List<Vector2i> positions)
    {
        movingOnPathEvent?.Invoke(StateModule.TransitionState, true);
        transitionPositions.AddRange(positions);
    }

    static SpriteMapObject()
    {
        DebugManager.AddLog(new DebugInfo("Map_Object_under_mouse"));
    }
}

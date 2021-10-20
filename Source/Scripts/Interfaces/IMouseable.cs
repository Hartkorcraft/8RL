using Godot;

public interface IMouseable
{
    void InputEvent(Node viepoint, InputEvent inputEvent, int local_shape);
    void OnMouseEnter();
    void OnMouseExit();
    void OnLeftMouseClick();
    void OnRightMouseClick();
}

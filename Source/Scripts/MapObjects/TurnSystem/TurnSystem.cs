
public class TurnSystem : ITurnable
{
    public int MovementPointsCap { get; set; } = 5;
    public int MovementPoints { get; set; } = 5;
    public virtual void StartTurn() { ResetMovementPoints(); }
    public virtual void EndTurn() { }
    public virtual void UpdateTurnObject() { }
    public void ResetMovementPoints() { MovementPoints = MovementPointsCap; }  //ITurnable
}

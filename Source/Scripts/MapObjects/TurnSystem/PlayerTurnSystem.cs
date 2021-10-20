
public class PlayerTurnSystem : TurnSystem
{
    PlayerCharacter parent;
    ActivitySystem? activitySystem;

    public override void UpdateTurnObject()
    {
        base.UpdateTurnObject();
        activitySystem?.UpdateCalculations();
    }

    public override void StartTurn()
    {
        base.StartTurn();
        activitySystem?.SetActivity(activitySystem.IdleActivity);
    }
    public override void EndTurn()
    {
        activitySystem?.SetActivity(activitySystem.NonTurnActivity);
    }

    public PlayerTurnSystem(PlayerCharacter _parent)
    {
        parent = _parent;
        activitySystem = parent.ActivitySystem;
    }
}

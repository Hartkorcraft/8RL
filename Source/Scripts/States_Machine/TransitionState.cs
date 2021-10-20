
public class TransitionState : GameState
{
    public override void Init()
    {
        CanSelect = false;
        AllowWorldInput = false;
        AllowActivityChange = false;
    }
    public TransitionState() { }
}

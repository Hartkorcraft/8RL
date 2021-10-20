
public interface ISelectable
{
    bool CanSelect { get; }
    void HandleSelection();
    void HandleBeingSelectedProcess();
    void HandleUnselection();
}

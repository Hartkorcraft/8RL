
public class PlayerHealthSystem : HealthSystem
{
    PlayerCharacter parent;



    public PlayerHealthSystem(PlayerCharacter player)
    {
        parent = player;
    }
}

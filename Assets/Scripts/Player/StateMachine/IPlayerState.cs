public enum IDPlayerState
{
    Ilde,
    Move,
    Sneaking,
    Falls,
    Jump,
}

public interface IPlayerState
{
    public IDPlayerState GetID();
    public void Enter(PlayerBase player);
    public void Update(PlayerBase player);
    public void Exit(PlayerBase player);
}

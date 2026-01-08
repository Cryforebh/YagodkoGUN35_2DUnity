public enum IDState
{
    IdleState,
    WalkState,
    PickUpState
}

public abstract class BotStateBase
{
    public abstract IDState GetIDState();
    public abstract void EnterState(BotBase bot);
    public abstract void UpdateState(BotBase bot);
    public abstract void ExitState(BotBase bot);
}

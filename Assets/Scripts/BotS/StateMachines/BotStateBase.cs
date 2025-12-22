using UnityEngine;

public abstract class BotStateBase 
{
    public abstract void EnterState(BotBase bot);

    public abstract void UpdateState(BotBase bot);
}

using UnityEngine;

public class BotWalkState : BotStateBase
{
    public override IDState GetIDState() => IDState.WalkState;

    public override void EnterState(BotBase bot)
    {
        bot.AllPathWalkContainer.UpdateDestination(bot.AIAgent);
    }

    public override void UpdateState(BotBase bot)
    {
        Moving(bot);
    }

    public override void ExitState(BotBase bot)
    {
        
    }

    private void Moving(BotBase bot)
    {
        bot.AIAnimator.SetFloat("Movement", bot.AIAgent.velocity.magnitude);
        
        if (bot.AIStateOfDanger == BotBase.StateOfDanger.Hostile)
        {
            bot.AIAgent.speed = 5f;
        }

        var distance = Vector3.Distance(bot.transform.position, bot.AIAgent.destination);
        if (distance <= bot.AIAgent.stoppingDistance)
        {
            bot.SwitchState(IDState.IdleState);
        }
    }
}

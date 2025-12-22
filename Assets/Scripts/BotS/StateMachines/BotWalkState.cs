using UnityEngine;

public class BotWalkState : BotStateBase
{
    private bool m_isCanWalk;

    public override void EnterState(BotBase bot)
    {
        m_isCanWalk = false;
        if (bot.IsCanWalk)
        {
            m_isCanWalk = true;
        }
    }

    public override void UpdateState(BotBase bot)
    {
        if (m_isCanWalk)
        {
            Moving(bot);
        }
    }

    private void Moving(BotBase bot)
    {
        bot.AIAnimator.SetFloat("Movement", bot.AIAgent.velocity.magnitude);

        var distance = Vector3.Distance(bot.transform.position, bot.AIAgent.destination);
        if (distance <= bot.AIAgent.stoppingDistance + 1f)
        {
            bot.AllPathWalkContainer.UpdateDestination(bot.AIAgent);
        }
    }
}

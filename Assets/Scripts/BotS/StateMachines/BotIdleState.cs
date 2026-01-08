using UnityEngine;

public class BotIdleState : BotStateBase
{
    private readonly float m_idleTime = 2f;
    private readonly float m_idleTimeExitAnimation = 0.5f;
    private float m_currentIdleTime = 0;

    public override IDState GetIDState() => IDState.IdleState;

    public override void EnterState(BotBase bot)
    {
        m_currentIdleTime = 0;
        bot.AIAnimator.SetBool("Thinks", true);
    }

    public override void UpdateState(BotBase bot)
    {
        bot.AIAnimator.SetFloat("Movement", bot.AIAgent.velocity.magnitude);
        m_currentIdleTime += Time.deltaTime;

        if (m_currentIdleTime >= m_idleTimeExitAnimation)
        {
            bot.AIAnimator.SetBool("Thinks", false);
        }

        if (m_currentIdleTime >= m_idleTime)
        {
            bot.SwitchState(IDState.WalkState);
        }
    }

    public override void ExitState(BotBase bot)
    {
    }
}

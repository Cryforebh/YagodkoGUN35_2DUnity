using UnityEngine;

public class BotPickUpState : BotStateBase
{
    private readonly float m_pickUpTime = 0.5f;
    private float m_currentPickUpTime = 0;

    public override IDState GetIDState() => IDState.PickUpState;

    public override void EnterState(BotBase bot)
    {
        Debug.Log("Это мы заберем!");
        bot.AIAnimator.SetBool("Collected", true);
        m_currentPickUpTime = m_pickUpTime;
    }

    public override void UpdateState(BotBase bot)
    {
        m_currentPickUpTime += Time.time;
        if (m_currentPickUpTime >= m_pickUpTime)
        {
            bot.AIAnimator.SetBool("Collected", false);
            bot.SwitchState(IDState.IdleState);
        }
    }

    public override void ExitState(BotBase bot)
    {
    }
}

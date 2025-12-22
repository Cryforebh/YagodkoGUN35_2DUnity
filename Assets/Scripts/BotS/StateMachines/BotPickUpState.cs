using UnityEngine;

public class BotPickUpState : BotStateBase
{
    private float m_time;
    private Client m_client;

    public override void EnterState(BotBase bot)
    {
        if (bot is Client clietn)
        {
            m_client = clietn;
        }
        else
        {
            m_client = null;
        }

        Debug.Log("Это мы заберем!");
        bot.AIAnimator.SetBool("Collected", true);
        m_time = 0.5f;
    }

    public override void UpdateState(BotBase bot)
    {
        m_time -= Time.deltaTime;
        if (m_time <= 0)
        {
            bot.AIAnimator.SetBool("Collected", false);

            if (m_client != null)
            {
                m_client.IsCanWalk = true;
                m_client.SwitchState(m_client.WalkState);
            }
            else
            {
                bot.SwitchState(bot.IdleState);
            }
        }
    }
}

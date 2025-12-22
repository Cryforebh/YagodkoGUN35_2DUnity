using UnityEngine;

public class BotIdleState : BotStateBase
{
    private Client m_clientCatch;

    public override void EnterState(BotBase bot)
    {
        if (bot is Client client)
        {
            m_clientCatch = client;
        }
        else
        {
            m_clientCatch = null;
        }
    }

    public override void UpdateState(BotBase bot)
    {
        if (m_clientCatch != null)
        {
            if (m_clientCatch.IsToTransfering)
            {
                m_clientCatch.IsToTransfering = false;
                m_clientCatch.SwitchState(m_clientCatch.IdlePickUpState);
            }
        }
    }
}

using UnityEngine;

public class BotStatePatrol : BotStateBase
{
    private Vector3 m_targetPosition;
    private Vector3 m_targetDirection;

    public override void EnterState(BotBase bot)
    {
        IsComplete = false;

        m_targetPosition = new Vector3(Random.Range(-15, 15), bot.transform.position.y, Random.Range(-15, 15));
        m_targetDirection = (m_targetPosition - bot.transform.position).normalized;
    }

    public override void ExitState(BotBase bot)
    {
        bot.SwitchState(bot.StateIdle);
    }

    public override void UpdateState(BotBase bot)
    {
        if (IsComplete == false)
        {
            if (Vector3.Distance(bot.transform.position, m_targetPosition) <= 1f)
                IsComplete = true;
        }
    }

    public override void FixedUpdateState(BotBase bot)
    {
        if (IsComplete == false)
        {
            bot.BotRigidbody.MovePosition(bot.transform.position + m_targetDirection * 3f * Time.fixedDeltaTime);
        }
    }
}

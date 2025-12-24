using UnityEngine;

public class BotStateDialogue : BotStateBase
{
    private Transform m_otherBotTransform;
    private int m_indexDialog;

    public override void EnterState(BotBase bot)
    {
        SetStartTimeState(3f);
        m_indexDialog = 0;
        IsComplete = false;
        bot.IsDialogue = true;
        m_otherBotTransform = bot.OtherNearBotsContainer[0];
    }

    public override void UpdateState(BotBase bot)
    {
        if (IsComplete == false)
        {

            if (m_indexDialog == 0)
            {
                m_indexDialog++;
                Debug.Log($"{bot.name}: Привет, {m_otherBotTransform.name}!");
            }

            if (GetCurrentTimeState() <= 0)
            {
                IsComplete = true;
            }
        }
    }

    public override void ExitState(BotBase bot)
    {
        if (IsComplete)
        {
            var index = Random.Range(0, 6);

            if (index % 2 == 0)
                bot.SwitchState(bot.StatePatrol);
            else
                bot.SwitchState(bot.StateIdle);
        }
    }
}

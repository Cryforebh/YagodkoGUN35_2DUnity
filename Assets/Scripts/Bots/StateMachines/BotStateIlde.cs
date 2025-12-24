using UnityEngine;

public class BotStateIlde : BotStateBase
{
    public override void EnterState(BotBase bot)
    {
        StartTime(3f,6f);
        IsComplete = false;

        // Тут можно запустить анимацию ожидания например:
        // * bot.Animator.Setbool("Idle",  true);
        // Или:
        // * bot.Animator.Play("Idle");
    }

    public override void UpdateState(BotBase bot)
    {
        if (IsComplete == false)
        {
            if (GetCurrentTimeState() <= 0)
            {
                IsComplete = true;
            }
        }
    }

    public override void ExitState(BotBase bot)
    {
        bot.SwitchState(bot.StatePatrol);
    }
}


using UnityEngine;

public abstract class BotStateBase
{
    /// <summary>
    /// Вовзращает статус состояния, или Устанавливает статус (Доступно в самих состояниях).
    /// </summary>
    public bool IsComplete { get; protected set; }
    private float m_currentTime = 5f;

    /// <summary>
    /// Реализовывает работу таймера, и возвращает текущее значение.
    /// </summary>
    /// <returns></returns>
    public float GetCurrentTimeState()
    {
        m_currentTime = m_currentTime - Time.deltaTime;
        return m_currentTime;
    }

    /// <summary>
    /// Устанавливает значение Таймера.
    /// </summary>
    public void SetStartTimeState(float time) => m_currentTime = time;

    /// <summary>
    /// Устанавливает значение Таймера от минимального до максимального.
    /// </summary>
    public void StartTime(float timeMin, float timeMax) => m_currentTime = Random.Range(timeMin, timeMax);



    /// <summary>
    /// Реализация работы состояния при входе в него.
    /// </summary>
    public abstract void EnterState(BotBase bot);

    /// <summary>
    /// Реализация работы состояния.
    /// </summary>
    public virtual void UpdateState(BotBase bot) { }

    /// <summary>
    /// Реализация работы состояния для физических обьектов.
    /// </summary>
    public virtual void FixedUpdateState(BotBase bot) { }

    /// <summary>
    /// Реализация работы состояния при выходе из него.
    /// </summary>
    public abstract void ExitState(BotBase bot);

}

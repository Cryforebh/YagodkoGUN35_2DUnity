using System.Threading;
using UnityEngine;

public class FallsPlayerState : IPlayerState
{
    private float m_timeOutAnimation = 0.1f;
    private float m_currentTime = 0;
    private bool m_isPlayingAnimation;

    public IDPlayerState GetID() => IDPlayerState.Falls;

    public void Enter(PlayerBase player)
    {
        Debug.Log(GetID());
        m_timeOutAnimation = 0;
        m_isPlayingAnimation = true;
        player.Animations?.AnimationOnGrounded(false);
        player.Animations?.AnimationFalls(m_isPlayingAnimation);
    }

    public void Exit(PlayerBase player)
    {

    }

    public void Update(PlayerBase player)

    {
        m_timeOutAnimation += Time.deltaTime;

        if (m_currentTime >= m_timeOutAnimation && m_isPlayingAnimation)
        {
            m_isPlayingAnimation = false;
            player.Animations?.AnimationFalls(m_isPlayingAnimation);
        }

        if (player.Velocity.y < 1.0f)
        {
            player.Animations?.AnimationOnGrounded(true);
            player.StateMachine.ChangeState(IDPlayerState.Ilde);
        }
    }
}

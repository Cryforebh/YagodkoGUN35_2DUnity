using UnityEngine;

public class JumpPlayerState : IPlayerState
{
    private float m_jumpForceDelayTime = 0.3f;
    private float m_currentDeleyTime = 0;

    public IDPlayerState GetID() => IDPlayerState.Jump;

    public void Enter(PlayerBase player)
    {
        Debug.Log(GetID());
        player.Animations.AnimationJump(true);
        m_currentDeleyTime = 0;
    }

    public void Exit(PlayerBase player)
    {
    }

    public void Update(PlayerBase player)
    {
        m_currentDeleyTime += Time.deltaTime;

        if (m_currentDeleyTime > m_jumpForceDelayTime)
        {
            player.Velocity.y = player.GetForceJump();
            player.StateMachine.ChangeState(IDPlayerState.Falls);
        }
    }
}

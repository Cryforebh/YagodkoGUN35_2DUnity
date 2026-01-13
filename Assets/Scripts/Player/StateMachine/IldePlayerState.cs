using UnityEngine;

public class IldePlayerState : IPlayerState
{
    public IDPlayerState GetID() => IDPlayerState.Ilde;

    public void Enter(PlayerBase player)
    {
        Debug.Log(GetID());
    }

    public void Exit(PlayerBase player)
    {
    }

    public void Update(PlayerBase player)
    {
        if (player.Velocity.y == 0)
        {
            if (player.Controls.PlayerMovement.Jump.ReadValue<float>() > 0)
                player.StateMachine.ChangeState(IDPlayerState.Jump);
        }
        else
        {
            if (player.Velocity.y > 1)
                player.StateMachine.ChangeState(IDPlayerState.Falls);
        }
    }
}

using UnityEngine;

public class MovePlayerState : IPlayerState
{
    private Vector3 m_currentTargetPosition;

    public IDPlayerState GetID() => IDPlayerState.Move;

    public void Enter(PlayerBase player)
    {
        Debug.Log(GetID());
    }

    public void Exit(PlayerBase player)
    {
    }

    public void Update(PlayerBase player)
    {
        var input = player.Controls.PlayerMovement.Moving.ReadValue<Vector3>();
        m_currentTargetPosition = GetDirection(player, input);

        player.Animations?.AnimationMove(m_currentTargetPosition);
        Rotation(player, m_currentTargetPosition);
        Moving(player, m_currentTargetPosition);
    }

    private Vector3 GetDirection(PlayerBase player, Vector3 inputPosition)
    {
        Vector3 forwardDirection = player.Camera.transform.forward;
        Vector3 rightDirection = player.Camera.transform.right;

        forwardDirection.y = 0;
        rightDirection.y = 0;

        forwardDirection = forwardDirection.normalized;
        rightDirection = rightDirection.normalized;

        player.TargetMoving = forwardDirection * inputPosition.z + rightDirection * inputPosition.x;

        Vector3 direction = Vector3.zero;

        if (player.Controls.PlayerMovement.Acceleration.ReadValue<float>() > 0)
            direction = player.TargetMoving.normalized * player.GetSpeedRun() * Time.deltaTime;
        else
            direction = player.TargetMoving.normalized * player.GetSpeedWalk() * Time.deltaTime;

        return direction;
    }

    private void Moving(PlayerBase player, Vector3 direction)
    {
        direction.y = player.CurrentForceGravity;
        player.CharacterControllerPlayer.Move(direction);
    }

    private void Rotation(PlayerBase player, Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * player.GetForceSpeedRotation());
        }
    }
}

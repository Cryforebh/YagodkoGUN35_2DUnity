using UnityEngine;

public class PlayerMovementRB
{
    private PlayerBase m_Player;
    private Vector3 m_currentTargetPosition;

    public PlayerMovementRB(PlayerBase player)
    {
        m_Player = player;
    }

    public void Update()
    {
        var input = m_Player.Controls.PlayerMovement.Moving.ReadValue<Vector3>();
        m_currentTargetPosition = GetDirection(m_Player, input);
        m_Player.Animations?.AnimationMove(m_currentTargetPosition);
        Rotation(m_currentTargetPosition);
    }

    public void FixedUpdate()
    {
        Moving(m_currentTargetPosition, Time.fixedDeltaTime);
        //GravityHandling();
    }

    public Vector3 GetDirection(PlayerBase player, Vector3 inputPosition)
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

    private void Moving(Vector3 direction, float fixedDeltaTime)
    {
        m_Player.RB.MovePosition(direction + m_Player.Velocity * fixedDeltaTime);
    }

    private void Rotation(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            m_Player.transform.rotation = Quaternion.Slerp(m_Player.transform.rotation, targetRotation, Time.deltaTime * m_Player.GetForceSpeedRotation());
        }
    }

    //public void GravityHandling()
    //{
    //    if (!IsGrounded() || m_Player.Velocity.y > 0)
    //    {
    //        m_Player.Velocity.y -= m_Player.GetGravityForce() * Time.deltaTime;
    //        Debug.Log("Тянем вниз.");
    //    }
    //    else
    //    {
    //        m_Player.Velocity.y = 0f;
    //    }
    //}

    public bool IsGrounded()
    {
        return Physics.CheckSphere(m_Player.GetGroundCheckerTransform().position, m_Player.GetGroundDistance(), m_Player.GetGroundMask());
    }
}

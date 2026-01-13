using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementСС : MonoBehaviour
{
    [SerializeField] private float m_speedWalk = 1.4f;
    [SerializeField] private float m_speedRun = 5f;
    [SerializeField] private float m_forceJump = 1f;
    [SerializeField] private float m_forceSpeedRotation = 10f;
    [SerializeField] private float m_gravityForce = 0.1f;

    private Camera m_camera;

    private PlayerControls m_playerControls;
    private CharacterController m_characterController;
    private PlayerAnimations m_playerAnimations;
    private Vector3 m_targetMoving;
    private float m_currentForceGravity;
    private bool m_isFalls;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_playerAnimations = GetComponent<PlayerAnimations>();
        m_characterController = GetComponent<CharacterController>();
        m_camera = Camera.main;

        m_playerControls = new PlayerControls();
        m_playerControls.PlayerMovement.Enable();
    }

    private void Update()
    {
        var direction = GetDirection();

        m_playerAnimations?.AnimationOnGrounded(!m_isFalls);
        Jump();

        if (IsGrounded())
        {
            //m_playerAnimations?.AnimationFalls(false);
            Rotation(direction);
            m_playerAnimations?.AnimationMove(direction);
        }
        else
        {
            //m_playerAnimations?.AnimationFalls(true);
        }

        Moving(direction);

        GravityHandling();
    }

    private void Moving(Vector3 direction)
    {
        direction.y = m_currentForceGravity;
        m_characterController.Move(direction);
    }

    private void Rotation(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_forceSpeedRotation);
        }
    }

    private Vector3 GetDirection()
    {
        var input = m_playerControls.PlayerMovement.Moving.ReadValue<Vector3>();

        Vector3 forwardDirection = m_camera.transform.forward;
        Vector3 rightDirection = m_camera.transform.right;

        forwardDirection.y = 0;
        rightDirection.y = 0;

        forwardDirection = forwardDirection.normalized;
        rightDirection = rightDirection.normalized;

        m_targetMoving = forwardDirection * input.z + rightDirection * input.x;

        Vector3 direction = Vector3.zero;

        if (m_playerControls.PlayerMovement.Acceleration.ReadValue<float>() > 0)
            direction = m_targetMoving.normalized * m_speedRun * Time.deltaTime;
        else
            direction = m_targetMoving.normalized * m_speedWalk * Time.deltaTime;

        return direction;
    }

    private void GravityHandling()
    {
        if (!IsGrounded())
        {
            m_currentForceGravity -= m_gravityForce * Time.deltaTime;

            if (m_currentForceGravity < 1f && !m_isFalls)
            {
                m_playerAnimations?.AnimationFalls(true);
                m_isFalls = true;
            }
        }
        else
        {
            m_playerAnimations?.AnimationFalls(false);
            m_isFalls = false;
            m_currentForceGravity = 0;
        }
    }

    private void Jump()
    {
        if (!IsGrounded()) return;
        if (m_playerControls.PlayerMovement.Jump.ReadValue<float>() > 0)
        {
            m_playerAnimations?.AnimationJump(true);
            m_currentForceGravity = m_forceJump * 0.1f;
        }
    }

    private bool IsGrounded() => m_characterController.isGrounded;

    private void OnDisable()
    {
        m_playerControls.PlayerMovement.Disable();
    }
}

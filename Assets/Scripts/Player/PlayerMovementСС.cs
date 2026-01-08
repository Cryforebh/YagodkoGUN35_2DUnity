using UnityEngine;

public class PlayerMovementСС : MonoBehaviour
{
    [SerializeField] private float m_speedWalk = 1.4f;
    [SerializeField] private float m_speedRun = 5f;
    [SerializeField] private float m_forceSpeedRotation = 10f;

    private Camera m_camera;
    private Animator m_animator;
    private PlayerControls m_playerControls;
    private CharacterController m_characterController;
    private Vector3 m_targetMoving;
    private float m_currentSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        m_camera = Camera.main;

        m_playerControls = new PlayerControls();
        m_playerControls.PlayerMovement.Enable();
    }

    private void Update()
    {
        var input = m_playerControls.PlayerMovement.Moving.ReadValue<Vector3>();
        var direction = GetDirection(input);

        Moving(direction);
        Rotation(direction);
        AnimationMove(direction);
    }

    private void Moving(Vector3 direction)
    {
        m_characterController.Move(direction);

        Rotation(m_targetMoving);
        AnimationMove(m_targetMoving);
    }

    private void Rotation(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_forceSpeedRotation);
        }
    }

    private void AnimationMove(Vector3 direction)
    {
        m_currentSpeed = direction.magnitude / Time.deltaTime;
        m_animator.SetFloat("Movement", m_currentSpeed);
    }

    private Vector3 GetDirection(Vector3 input)
    {
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
}

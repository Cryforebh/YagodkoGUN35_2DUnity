using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_speedWalk = 1.4f;
    [SerializeField] private float m_speedRun = 5f;
    [SerializeField] private float m_forceSpeedRotation = 10f;

    private PlayerControls m_inputAction;
    private Animator m_animator;
    private Rigidbody m_rigidbody;
    private Vector3 m_targetPosition;
    private Camera m_camera;
    private Vector3 m_cameraPosition;

    private float m_currentSpeed;
    private bool m_isRun;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_camera = Camera.main;
        m_inputAction = new PlayerControls();
        m_inputAction.PlayerMovement.Enable();
        m_inputAction.PlayerMovement.Moving.canceled += Moving_canceled;
        m_inputAction.PlayerMovement.Acceleration.performed += Acceleration_performed;
        m_inputAction.PlayerMovement.Acceleration.canceled += Acceleration_canceled;
    }

    private void OnDisable()
    {
        m_inputAction.PlayerMovement.Moving.canceled -= Moving_canceled;
        m_inputAction.PlayerMovement.Acceleration.performed -= Acceleration_performed;
        m_inputAction.PlayerMovement.Acceleration.canceled -= Acceleration_canceled;
        m_inputAction.PlayerMovement.Disable();
    }

    private void Acceleration_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) => m_isRun = false;

    private void Acceleration_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => m_isRun = true;

    private void Moving_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) => m_targetPosition = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 inputVector = m_inputAction.PlayerMovement.Moving.ReadValue<Vector3>();

        Vector3 forwardDirection = m_camera.transform.forward;
        Vector3 rightDirection = m_camera.transform.right;

        forwardDirection.y = 0;
        rightDirection.y = 0;

        forwardDirection = forwardDirection.normalized;
        rightDirection = rightDirection.normalized;

        m_targetPosition = forwardDirection * inputVector.z + rightDirection * inputVector.x;

        Vector3 moveVector;
        if (m_isRun)
        {
            moveVector = m_targetPosition.normalized * m_speedRun * Time.fixedDeltaTime;
        }
        else
        {
            moveVector = m_targetPosition.normalized * m_speedWalk * Time.fixedDeltaTime;
        }

        // ѕоворачиваем персонажа в направлении движени€
        if (m_targetPosition != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * m_forceSpeedRotation);
        }

        m_rigidbody.MovePosition(m_rigidbody.position + moveVector);

        m_currentSpeed = moveVector.magnitude / Time.fixedDeltaTime;

        m_animator.SetFloat("Movement", m_currentSpeed);
    }
}

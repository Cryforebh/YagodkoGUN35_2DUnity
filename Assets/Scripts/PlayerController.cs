using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Настройки камеры:")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _horizontalTurnSensitivity = 10f; // X Чувствительность по горизонтали
    [SerializeField] private float _verticalTurnSensitivity = 10f; // Y Чувствительность по вертикали
    [SerializeField] private float _verticalMinAngle = -60f;    // Минимальный угол по вертикали
    [SerializeField] private float _verticalMaxAngle = 60f;     // Максимальный угол по вертикали
    [Header("Настройки движения персонажа:")]
    [SerializeField] private float _moveSpeed = 5f; // Скорость движения персонажа

    private Rigidbody _rigidbody;
    private InputControl _playerInput;

    private Vector2 _mouseInput = Vector2.zero;
    private Vector3 _moveInput = Vector2.zero;

    private float _rotationY = 0f;   // Текущий угол по вертикали

    private void Awake()
    {
        if (!_camera) Debug.LogWarning("Not camera!");

        _playerInput = new InputControl();
        _rigidbody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable() => _playerInput.Enable();

    private void Start()
    {
        _playerInput.PlayerMove.WASD.performed += Move_performed;
        _playerInput.CameraControls.MouseDelta.performed += Camera_performed;
    }

    private void Camera_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
        _mouseInput += obj.ReadValue<Vector2>();

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
        _moveInput = new Vector3(obj.ReadValue<Vector3>().x, 0, obj.ReadValue<Vector3>().z);

    private void FixedUpdate() => ProcessMovement();

    private void Update() => ProcessCameraRotation();

    private void ProcessMovement()
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.z);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.Normalize();

        Vector3 targetPosition = _rigidbody.position + moveDirection * _moveSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(targetPosition);
    }

    private void ProcessCameraRotation()
    {
        float rotationX = _mouseInput.x * _horizontalTurnSensitivity * Time.deltaTime;
        transform.Rotate(0,rotationX,0);

        _rotationY -= _mouseInput.y * _verticalTurnSensitivity * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, _verticalMinAngle, _verticalMaxAngle);

        _camera.transform.localEulerAngles = new Vector3(_rotationY,0,0);
        _mouseInput = Vector2.zero;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.PlayerMove.WASD.performed -= Move_performed;
        _playerInput.CameraControls.MouseDelta.performed -= Camera_performed;
    }
    private void OnDestroy() => OnDisable();
}

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Настройки камеры:")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _horizontalTurnSensitivity = 10f; // X Чувствительность по горизонтали
    [SerializeField] private float _verticalTurnSensitivity = 10f; // Y Чувствительность по вертикали
    [SerializeField] private float _verticalMinAngle = -60f;    // Минимальный угол по вертикали
    [SerializeField] private float _verticalMaxAngle = 60f;     // Максимальный угол по вертикали
    [Header("Настройки движения персонажа:")]
    [SerializeField] private float moveSpeed = 5f; // Скорость движения персонажа

    private float _mouseInputX = 0f;
    private float _mouseInputY = 0f;
    private float _moveInputX = 0f;
    private float _moveInputZ = 0f;
    private Rigidbody _rigidbody;
    private InputControl _playerInput;
    private Vector3 _moveDirection;

    private float _rotationY = 0f;   // Текущий угол по вертикали

    private void Awake()
    {
        _playerInput = new InputControl();
        _rigidbody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _playerInput.PlayerMove.WASD.performed += Move_performed;
        _playerInput.CameraControls.MouseDelta.performed += Camera_performed;

        StartCoroutine(ProcessCheckDirectionAndMove());
    }

    private void Camera_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        TurnCamera(obj.ReadValue<Vector2>() * Time.deltaTime);
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        MoveCharacter(obj.ReadValue<Vector3>());
    }

    private void TurnCamera(Vector2 mouseInput)
    {
        _mouseInputX = mouseInput.x * 0.1f;
        _mouseInputY = mouseInput.y * 0.1f;
    }

    private void MoveCharacter(Vector3 moveInput)
    {
        _moveInputX = moveInput.x * 0.1f;
        _moveInputZ = moveInput.z * 0.1f;
    }

    private IEnumerator ProcessCheckDirectionAndMove()
    {
        while (true)
        {
            //// Определяем направление движения относительно камеры
            _moveDirection = new Vector3(_moveInputX, 0, _moveInputZ);
            _moveDirection = transform.TransformDirection(_moveDirection);

            // Камера            
            _rotationY -= _mouseInputY * _verticalTurnSensitivity /** Time.deltaTime*/;
            _rotationY = Mathf.Clamp(_rotationY, _verticalMinAngle, _verticalMaxAngle);

            transform.Rotate(0, _mouseInputX * _horizontalTurnSensitivity /** Time.deltaTime*/, 0);

            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x = _rotationY;
            _camera.transform.eulerAngles = eulerAngles;

            // Применяем движение к персонажу
            _rigidbody.velocity = (_moveDirection * moveSpeed);

            yield return null;
        }
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.PlayerMove.WASD.performed -= Move_performed;
        _playerInput.CameraControls.MouseDelta.performed -= Camera_performed;
    }
}

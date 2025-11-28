using UnityEngine;

public class PlayerMoveSideView : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SideControl _sideControl;

    public float _forceSpeed;
    public float _jumpForce;

    private bool _isJump = false;
    private bool _isLeft = false;
    private bool _isRight = false;

    private Vector2 _movement = Vector2.zero;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sideControl = new SideControl();
    }

    private void OnEnable()
    {
        _sideControl.Game.Enable();

        _sideControl.Game.Left.performed += Left_performed;
        _sideControl.Game.Left.canceled += Left_canceled;
        _sideControl.Game.Rigth.performed += Rigth_performed;
        _sideControl.Game.Rigth.canceled += Rigth_canceled;
        _sideControl.Game.Up.started += Up_started;
    }
    private void OnDisable()
    {
        _sideControl.Game.Left.performed -= Left_performed;
        _sideControl.Game.Left.canceled -= Left_canceled;
        _sideControl.Game.Rigth.performed -= Rigth_performed;
        _sideControl.Game.Rigth.canceled -= Rigth_canceled;
        _sideControl.Game.Up.started -= Up_started;
    }

    private void Up_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isJump = true;
    }

    private void Rigth_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isRight = false;
    }

    private void Rigth_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isRight = true;
    }

    private void Left_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isLeft = true;
    }

    private void Left_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isLeft = false;
    }

    private void Reset()
    {
        _isJump = false;
        _isLeft = false;
        _isRight = false;
    }


    private void FixedUpdate()
    {
        Vector2 movement = Vector2.zero;

        if (_isLeft)
        {
            movement.x = -1;
        }
        if (_isRight)
        {
            movement.x = 1;
        }

        _rb.AddForce(movement * _forceSpeed * 0.5f, ForceMode2D.Impulse);

        if (_isJump)
        {
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            _isJump = false;
        }

        // Поворот спрайта
        if (_isLeft)
        {
            transform.localScale = new Vector2(-1, 1) * Mathf.Abs(transform.localScale.x);
        }
        else if (_isRight)
        {
            transform.localScale = new Vector2(1, 1) * Mathf.Abs(transform.localScale.x);
        }
    }
}

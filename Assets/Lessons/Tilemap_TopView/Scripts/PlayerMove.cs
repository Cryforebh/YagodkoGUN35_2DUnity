using System;
using System.Collections;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _player;
    private TopControl _inputActions;
    private bool _isUp = false;
    private bool _isLeft = false;
    private bool _isRight = false;
    private bool _isDown = false;
    public float moveSpeed = 5f; // Скорость движения
    public float turnSpeed = 10f; // Скорость поворота

    public event Action MoveEvent;
    public event Action DontMoveEvent;

    private void Awake()
    {
        _inputActions = new TopControl();
    }

    private void OnEnable()
    {
        _inputActions.Game.Enable();

        _inputActions.Game.Up.performed += Move_Up;
        _inputActions.Game.Up.canceled += Move_Up_Stop;
        _inputActions.Game.Down.performed += Move_Down;
        _inputActions.Game.Down.canceled += Move_Down_Stop;
        _inputActions.Game.Left.performed += Move_Left;
        _inputActions.Game.Left.canceled += Move_Left_Stop;
        _inputActions.Game.Right.performed += Move_Right;
        _inputActions.Game.Right.canceled += Move_Right_Stop;
    }
    private void OnDisable() 
    {
        _inputActions.Game.Disable();

        _inputActions.Game.Up.performed -= Move_Up;
        _inputActions.Game.Up.canceled -= Move_Up_Stop;
        _inputActions.Game.Down.performed -= Move_Down;
        _inputActions.Game.Down.canceled -= Move_Down_Stop;
        _inputActions.Game.Left.performed -= Move_Left;
        _inputActions.Game.Left.canceled -= Move_Left_Stop;
        _inputActions.Game.Right.performed -= Move_Right;
        _inputActions.Game.Right.canceled -= Move_Right_Stop;
    }

    private void Move_Up(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        MoveReset();
        _isUp = true;
        ActionMoveSignal();
    }
    private void Move_Up_Stop(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isUp = false;
        if (!IsMove()) ActionStopSignal();
    }

    private void Move_Down(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        MoveReset();
        _isDown = true;
        ActionMoveSignal();
    }
    private void Move_Down_Stop(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isDown = false;
        if (!IsMove()) ActionStopSignal();
    }

    private void Move_Left(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        MoveReset();
        _isLeft = true;
        ActionMoveSignal();
    }
    private void Move_Left_Stop(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        _isLeft = false;
        if (!IsMove()) ActionStopSignal();
    }

    private void Move_Right(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        MoveReset();
        _isRight = true;
        ActionMoveSignal();
    }
    private void Move_Right_Stop(UnityEngine.InputSystem.InputAction.CallbackContext obj) 
    {
        _isRight = false;
        if (!IsMove()) ActionStopSignal();
    }

    private void MoveReset()
    {
        _isUp = false;
        _isLeft = false;
        _isRight = false;
        _isDown = false;
    }

    private bool IsMove()
    {
        if (_isDown || _isLeft || _isRight || _isUp) return true;
        else return false;
    }

    private void FixedUpdate()
    {
        Vector2 movement = Vector2.zero;

        if (_isUp) movement += Vector2.up;
        if (_isDown) movement += Vector2.down;
        if (_isLeft) movement += Vector2.left;
        if (_isRight) movement += Vector2.right;

        _player.velocity = movement * moveSpeed;

        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.x, -movement.y) * Mathf.Rad2Deg;
            _player.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void ActionMoveSignal() => MoveEvent?.Invoke();
    private void ActionStopSignal() => DontMoveEvent?.Invoke();

}

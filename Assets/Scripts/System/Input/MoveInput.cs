using System;
using UnityEngine;

public class MoveInput : IDisposable
{
    private InputControl _input;

    public MoveInput()
    {
        _input = new InputControl();
        _input.PlayerMap.Enable();
    }

    public void Dispose()
    {
        _input.PlayerMap.Disable();
    }

    public Vector2 GetMovement() => _input.PlayerMap.Movement.ReadValue<Vector2>();
}

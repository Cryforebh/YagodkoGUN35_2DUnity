using System;
using UnityEngine;

public class MoveInput
{
    private readonly InputControl _input;

    public MoveInput()
    {
        _input = new InputControl();
        _input.PlayerMap.Enable();
    }

    public Vector2 GetMovement() => _input.PlayerMap.Movement.ReadValue<Vector2>();
}

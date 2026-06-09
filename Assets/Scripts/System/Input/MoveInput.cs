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
    public bool IsDownBottonNextLevel() => _input.Menu.Nextscene.ReadValue<float>() > 0.5f;
}

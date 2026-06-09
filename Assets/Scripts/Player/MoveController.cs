using UnityEngine;
using Zenject;

public class MoveController : ITickable
{
    private Vector3 _targetPosition;
    private ICharacter _character;
    private MoveInput _input;

    public MoveController(MoveInput input, ICharacter character)
    {
        _input = input;
        _character = character;
    }

    public void Tick() // Update from ITickable for Zenject
    {
        Movement();
    }

    private void Movement()
    {
        if (_character == null) return;

        float x = _input.GetMovement().normalized.x;
        float z = _input.GetMovement().normalized.y;
        _targetPosition = new Vector3(x, 0, z);

        _character.SetTargetPosition(_targetPosition * Time.deltaTime * _character.SpeedMove);
    }
}

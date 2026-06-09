using UnityEngine;
using Zenject;

public class Character : MonoBehaviour, ICharacter
{
    [SerializeField] private float _speedMove = 4;

    private Vector3 _targetPosition;
    private CharacterController _controller;
    private MoveInput _input;

    [Inject]
    private void Construct(MoveInput input)
    {
        _input = input;
        _controller = GetComponent<CharacterController>();
    }

    public void ManualUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float x = _input.GetMovement().normalized.x;
        float z = _input.GetMovement().normalized.y;
        _targetPosition = new Vector3(x, 0, z);

        _controller.Move(_targetPosition * Time.deltaTime * _speedMove);
    }
}

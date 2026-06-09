using UnityEngine;

public class MoveInput : MonoBehaviour
{
    private InputControl _input;

    private void Start()
    {
        _input = new InputControl();
        _input.PlayerMap.Enable();
    }

    public Vector2 GetMovement() => _input.PlayerMap.Movement.ReadValue<Vector2>();

    private void OnDisable() => _input.PlayerMap.Disable();
}

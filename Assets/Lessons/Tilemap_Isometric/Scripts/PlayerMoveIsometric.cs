using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMoveIsometric : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Camera _camera;
    private ControlIsometric _control;
    private Tilemap _map;
    private Vector3 _targetPosition;

    public float MoveSpeed = 1.0f;

    private void Awake()
    {
        _control = new ControlIsometric();
        _map = GetComponent<Tilemap>();
    }

    private void OnEnable()
    {
        _control.Enable();
        _control.Game.MouseLeftClick.performed += MouseLeftClick_performed;
    }

    private void OnDisable()
    {
        _control.Disable();
        _control.Game.MouseLeftClick.performed -= MouseLeftClick_performed;
    }

    private void MouseLeftClick_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector3 screenPosition = Input.mousePosition;
        _targetPosition = _camera.ScreenToWorldPoint(screenPosition);

        _player.transform.position = _targetPosition + new Vector3(0,0,10);
    }

}

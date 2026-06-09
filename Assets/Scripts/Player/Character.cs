using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Character : MonoBehaviour, ICharacter
{
    [SerializeField] private float _speedMove = 4;

    private CharacterController _controller;

    public float SpeedMove => _speedMove;

    public event Action OnDeath; 

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _controller.Move(targetPosition);
    }

    [ContextMenu("Death")]
    public void Death()
    {
        OnDeath?.Invoke();
    } 
}

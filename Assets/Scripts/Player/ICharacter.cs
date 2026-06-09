using System;
using UnityEngine;

public interface ICharacter 
{
    event Action OnDeath;
    float SpeedMove { get; }
    void SetTargetPosition(Vector3 targetPosition);
    void Death();
}

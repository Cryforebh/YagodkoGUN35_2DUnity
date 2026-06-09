using UnityEngine;

public interface ICharacter 
{
    float SpeedMove { get; }
    void SetTargetPosition(Vector3 targetPosition);
}

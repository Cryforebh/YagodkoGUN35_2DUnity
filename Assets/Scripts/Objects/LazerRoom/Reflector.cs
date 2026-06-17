using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    public float rotationStep = 90f; // Шаг поворота
    private int currentRotation = 0;

    public Vector3 GetNormal()
    {
        float angle = currentRotation * rotationStep;
        Quaternion rot = Quaternion.Euler(0, angle, 0);
        return rot * Vector3.up; // Нормаль зависит от ориентации
    }

    public void Rotate()
    {
        currentRotation = (currentRotation + 1) % 4;
        transform.rotation = Quaternion.Euler(0, currentRotation * rotationStep, 0);
    }
}

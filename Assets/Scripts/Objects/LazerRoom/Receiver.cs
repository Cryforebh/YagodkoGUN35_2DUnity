using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    public Color expectedColor;
    public bool isActive;

    public void Activate()
    {
        isActive = true;
        // Визуализация активации (например, изменение цвета)
    }
}

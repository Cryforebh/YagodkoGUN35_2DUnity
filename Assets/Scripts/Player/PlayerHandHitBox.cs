using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandHitBox : MonoBehaviour
{
    public event Action<Collider> OnHitBotEnterEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bot")
        {
            print("Коснулась");
            OnHitBotEnterEvent?.Invoke(other);
        }
    }
}

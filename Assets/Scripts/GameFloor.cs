using System;
using UnityEngine;

public class GameFloor : MonoBehaviour
{
    public event Action CollisionEnterEvent;
    public event Action CollisionStayEvent;
    public event Action CollisionExitEvent;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            CollisionEnterEvent?.Invoke();

            Debug.Log("ћ€ч упал");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            CollisionStayEvent?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            CollisionExitEvent?.Invoke();
        }
    }
}

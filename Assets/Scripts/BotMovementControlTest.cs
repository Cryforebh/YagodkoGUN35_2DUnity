using DG.Tweening;
using UnityEngine;

public class BotMovementControlTest : MonoBehaviour
{
    [SerializeField] private float m_slowingSpeed = 2f;

    private Vector3 m_position;
    private Vector3 m_direction = Vector3.zero;
    private GameControls m_controls;

    private void OnEnable()
    {
        m_controls = new();
        m_controls.Enable();
        m_controls.PlayerMovementMap.Moving.performed += ProcessMoving;
        m_position = transform.position;
    }

    private void ProcessMoving(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        m_direction = obj.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        m_controls.PlayerMovementMap.Moving.performed -= ProcessMoving;
        m_controls.Disable();
    }

    private void Update()
    {
        ProcessRotate();
        ProcessMove();
        ProcessScale();
    }

    private void ProcessRotate()
    {
        if (m_direction.x < 0)
            transform.DORotate(new Vector3(0, 270, 0), 0.5f, RotateMode.Fast);
        else
        {
            transform.DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.Fast);
        }
    }

    private void ProcessMove()
    {
        transform.DOMoveX(m_position.x + m_direction.x * 2f, m_slowingSpeed);
        m_position = transform.position;
    }

    private void ProcessScale()
    {
        if (transform.localScale.x > 1.45f)
        {
            transform.DOScale(new Vector3(1, 1, 1), m_slowingSpeed);
        }
        else if (transform.localScale.x < 1.05f)
        {
            transform.DOScale(new Vector3(1.5f, 1, 1.5f), m_slowingSpeed);
        }
    }
}

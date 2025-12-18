using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private bool m_animatorActive = false;

    private NavMeshAgent m_navMeshAgent;
    private InputControls m_inputControls;

    private Ray m_Ray;
    private RaycastHit m_raycastHit;
    private Vector3 m_targetPosition;
    private float m_Speed;

    private const float MaxDistance = 3.5f;
    private const float MinDistance = 1f;
    private Animator m_animator;

    private void Awake()
    {
        m_inputControls = new InputControls();
        m_inputControls.Enable();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_targetPosition = transform.position;
        if (m_animatorActive)
            m_animator = GetComponent<Animator>();
        m_Speed = m_navMeshAgent.speed;
    }

    private void OnEnable()
    {
        m_inputControls.PlayerMap.MousePosition.performed += MousePosition_performed;
        m_inputControls.PlayerMap.MoveDestenationClick.canceled += MoveDestenationClick_canceled;
    }

    private void MoveDestenationClick_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (Physics.Raycast(m_Ray, out m_raycastHit))
        {
            if (m_raycastHit.collider.tag == "Plane")
            {
                m_targetPosition = m_raycastHit.point;
            }
        }

        m_navMeshAgent.destination = m_targetPosition;

        if (m_navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
        }
    }

    private void MousePosition_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
    }

    private void Update()
    {
        m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        AnimationUpdate();

        //m_animator.SetFloat("Speed", m_navMeshAgent.velocity.magnitude);
    }

    private void AnimationUpdate()
    {
        float distance = Vector3.Distance(transform.position, m_targetPosition);

        if (m_animatorActive)
        {
            if (distance >= MaxDistance)
            {
                SetAnimatorSpeed(m_navMeshAgent.velocity.magnitude);
                m_navMeshAgent.speed = m_Speed;
            }
            else if (distance > MinDistance)
            {
                SetAnimatorSpeed(0.2f * distance);
                m_navMeshAgent.speed = m_Speed / 2f;
            }
            else
            {
                SetAnimatorSpeed(0);
                m_navMeshAgent.speed = 0;
            }
        }
        else
        {
            m_navMeshAgent.destination = m_targetPosition;
        }
    }

    private void SetAnimatorSpeed(float speed)
    {
        if (m_animatorActive)
            m_animator.SetFloat("Speed", speed);
    }

    private void OnDisable()
    {
        m_inputControls.PlayerMap.MousePosition.canceled -= MousePosition_performed;
        m_inputControls.PlayerMap.MoveDestenationClick.canceled -= MoveDestenationClick_canceled;
        m_inputControls.PlayerMap.Disable();
    }

    private void OnDrawGizmos()
    {
        //if (m_debagRay)
        //{
        //    Gizmos.color = Color.red;

        //        Gizmos.DrawRay(m_Ray.origin, m_Ray.direction * 20f);
        //        if (Physics.Raycast(m_Ray, out m_raycastHit))
        //        {
        //            if (m_raycastHit.collider.tag == "Plane")
        //            {
        //                m_targetPosition = m_raycastHit.point;
        //            }
        //        }
        //        Gizmos.DrawSphere(m_targetPosition, 0.2f);
        //}
    }
}

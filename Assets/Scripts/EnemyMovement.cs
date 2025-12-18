using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform m_targetTransform;
    private NavMeshAgent m_navMeshAgent;
    private Animator m_animator;
    private float m_Speed;

    private const float MaxDistance = 5f;
    private const float MinDistance = 2f;

    private void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        m_Speed = m_navMeshAgent.speed;

    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, m_targetTransform.position);

        if (distance >= MaxDistance)
        {
            SetAnimatorSpeed(m_navMeshAgent.velocity.magnitude);
            m_navMeshAgent.speed = m_Speed;
        }
        else if (distance > MinDistance)
        {
            SetAnimatorSpeed(0.15f * distance);
            m_navMeshAgent.speed = m_Speed / 4f;
        }
        else
        {
            SetAnimatorSpeed(0);
            m_navMeshAgent.speed = 0;
        }

        m_navMeshAgent.destination = m_targetTransform.position;
    }

    private void SetAnimatorSpeed(float speed)
    {
        m_animator.SetFloat("Speed", speed);
    }
}

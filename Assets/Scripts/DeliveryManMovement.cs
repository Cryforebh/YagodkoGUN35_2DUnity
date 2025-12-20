using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class DeliveryManMovement : MonoBehaviour
{
    private NavMeshAgent m_meshAgent;
    private Animator m_animator;

    private void Awake()
    {
        m_meshAgent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }

    public void AssignTarget(Transform target)
    {
        m_meshAgent.destination = target.position;
    }

    public void ProcessOfTransferringProducts(bool isNearWithClient)
    {
        if (isNearWithClient)
        {
            
        }
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class DeliveryMan : MonoBehaviour
{
    private NavMeshAgent m_meshAgent;
    private Animator m_animator;
    private AudioSource m_audioSource;
    private WaitForSeconds m_delayTransfer = new WaitForSeconds(1);
    private WaitForSeconds m_delayTransferAnimation = new WaitForSeconds(0.2f);

    public event Action ReadyToMoveEvent;
    public event Action StartTransferEvent;

    public NavMeshAgent MeshAgent => m_meshAgent;
    public AudioSource AudioSource => m_audioSource;

    private void Awake()
    {
        m_meshAgent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        SetAnimatorMovement();
    }

    public void AssignTarget(Transform target)
    {
        m_meshAgent.destination = target.position;
    }

    public void ProcessOfTransferringProducts(bool isNearWithClient)
    {
        if (isNearWithClient)
        {
            StartCoroutine(DelayTransfer());
        }
    }

    public void SetAnimatorMovement()
    {
        m_animator.SetFloat("Movement", m_meshAgent.velocity.magnitude);
    }

    private IEnumerator DelayTransfer()
    {
        m_animator.SetBool("Transfer", true);
        StartTransferEvent?.Invoke();

        yield return m_delayTransferAnimation;

        m_animator.SetBool("Transfer", false);

        yield return m_delayTransfer;

        ReadyToMoveEvent?.Invoke();
        print("Готов к следующему заказу!");
    }
}

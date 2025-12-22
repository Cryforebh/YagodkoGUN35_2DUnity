using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class DeliveryMan : BotBase
{
    private WaitForSeconds m_delayTransfer = new WaitForSeconds(1);
    private WaitForSeconds m_delayTransferAnimation = new WaitForSeconds(0.2f);

    public event Action ReadyToMoveEvent;
    public event Action StartTransferEvent;

    private void Awake()
    {
        AIAnimator = GetComponent<Animator>();
        AIAudioSource = GetComponent<AudioSource>();
        AIAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        SetAnimatorMovement();
    }

    public void AssignTarget(Transform target)
    {
        AIAgent.destination = target.position;
    }

    public void ProcessOfTransferringProducts(bool isNearWithClient, Client client)
    {
        if (isNearWithClient)
        {
            client.IsToTransfering = true;
            StartCoroutine(DelayTransfer());
        }
    }

    public void SetAnimatorMovement()
    {
        AIAnimator.SetFloat("Movement", AIAgent.velocity.magnitude);
    }

    private IEnumerator DelayTransfer()
    {
        AIAnimator.SetBool("Transfer", true);
        StartTransferEvent?.Invoke();

        yield return m_delayTransferAnimation;

        AIAnimator.SetBool("Transfer", false);

        yield return m_delayTransfer;

        ReadyToMoveEvent?.Invoke();
        print("Готов к следующему заказу!");
    }
}

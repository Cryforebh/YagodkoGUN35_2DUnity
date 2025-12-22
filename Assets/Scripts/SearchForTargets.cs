using System.Collections.Generic;
using UnityEngine;

public class SearchForTargets : MonoBehaviour
{
    [SerializeField] private float m_distanceInteractionWithClient = 3f;
    [SerializeField] private DeliveryMan m_deliveryMan;
    [SerializeField] private VoiceContainer m_voiceContainer;

    private float m_maxDistance = 300f;
    private List<Client> m_botClients = new List<Client>();
    private Client m_botTarget;
    private bool m_isFindingClientEnd = false;
    private bool m_isDeliveryManNearWithClient = false;
    private bool m_isCompliteDelivery = false;

    private void Awake()
    {
        AddAllClientsToContainer();
    }

    private void Start()
    {
        ProcessWorkDelivery();
        m_deliveryMan.ReadyToMoveEvent += ProcessWorkDelivery;
        m_deliveryMan.StartTransferEvent += StartTransferProcess;
    }

    private void AddAllClientsToContainer()
    {
        var clients = GetComponentsInChildren<Client>();

        foreach (var client in clients)
        {
            m_botClients.Add(client);
        }
    }

    private void StartTransferProcess()
    {
        if (m_voiceContainer)
            m_voiceContainer.VoiceDeliveryManPlay(m_deliveryMan);
    }

    private void Update()
    {
        RunToClient(m_deliveryMan, m_botTarget);
    }

    private void ProcessWorkDelivery()
    {
        if (m_botTarget && m_voiceContainer)
        {
            m_voiceContainer.VoiceClientPlay(m_botTarget);
            m_botClients.Remove(m_botTarget);
        }

        m_isDeliveryManNearWithClient = false;
        FindingNearestClient(m_deliveryMan.transform);
    }

    private void FindingNearestClient(Transform deliveryMan)
    {
        if (m_isCompliteDelivery) return;

        float distanceNearest = m_maxDistance;

        foreach (Client client in m_botClients)
        {
            if (client.IsClient)
            {
                var distance = GetDistance(deliveryMan, client.transform);
                if (distance < distanceNearest)
                {
                    distanceNearest = distance;
                    m_botTarget = client;
                }
            }
        }

        if (m_botClients.Count <= 0)
            m_isCompliteDelivery = true;

        m_isFindingClientEnd = true;
    }

    private void RunToClient(DeliveryMan deliveryMan, Client client)
    {
        if (m_isCompliteDelivery) return;
        if (m_isDeliveryManNearWithClient) return;

        if (m_isFindingClientEnd && m_botTarget)
        {
            var velocity = deliveryMan.AIAgent.velocity.magnitude;

            if (m_distanceInteractionWithClient <= GetDistance(deliveryMan.transform, client.transform))
            {
                deliveryMan.AssignTarget(client.transform);
            }
            else if (velocity <= 0.0f)
            {
                m_isDeliveryManNearWithClient = true;
                client.IsClient = false;
                m_isFindingClientEnd = false;
                deliveryMan.ProcessOfTransferringProducts(m_isDeliveryManNearWithClient,client);
            }
        }
    }

    private float GetDistance(Transform pointOne, Transform pointTwo)
    {
        return Vector3.Distance(pointOne.position, pointTwo.position);
    }

    private void OnDisable()
    {
        m_deliveryMan.ReadyToMoveEvent -= ProcessWorkDelivery;
        m_deliveryMan.StartTransferEvent -= StartTransferProcess;
    }
}

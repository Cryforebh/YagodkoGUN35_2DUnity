using UnityEditor.PackageManager;
using UnityEngine;

public class SearchForTargets : MonoBehaviour
{
    [SerializeField] private float m_distanceInteractionWithClient = 3f;
    [SerializeField] private DeliveryManMovement m_deliveryMan;
    private float m_maxDistance = 300f;
    private BotClient[] m_botClients;
    private BotClient m_botTarget;
    private bool m_isFindingClientEnd = false;
    private bool m_isDeliveryManNearWithClient = false;

    private void Awake()
    {
        m_botClients = GetComponentsInChildren<BotClient>();
    }

    private void Start()
    {
        ProcessWorkDelivery();
    }

    private void Update()
    {
        RunToClient(m_deliveryMan, m_botTarget);
    }

    private void ProcessWorkDelivery()
    {
        FindingNearestClient(m_deliveryMan.transform);
    }

    private void FindingNearestClient(Transform deliveryMan)
    {
        float distanceNearest = m_maxDistance; 

        foreach (BotClient client in m_botClients)
        {
            if (!client.IsClient) return;

            var distance = GetDistance(deliveryMan, client.transform);
            if (distance < distanceNearest)
            {
                distanceNearest = distance;
                m_botTarget = client; 
            }
        }

        m_isFindingClientEnd = true;
    }

    private void RunToClient(DeliveryManMovement deliveryMan, BotClient client)
    {
        if (m_isDeliveryManNearWithClient) return;

        if (m_isFindingClientEnd && m_botTarget) 
        {
            if (m_distanceInteractionWithClient <= GetDistance(deliveryMan.transform, client.transform))
            {
                deliveryMan.AssignTarget(client.transform);
            }
            else
            {
                m_isDeliveryManNearWithClient = true;
                deliveryMan.ProcessOfTransferringProducts(m_isDeliveryManNearWithClient);
            }
        }
    }

    private float GetDistance(Transform pointOne, Transform pointTwo)
    {
        return Vector3.Distance(pointOne.position, pointTwo.position);
    }
}

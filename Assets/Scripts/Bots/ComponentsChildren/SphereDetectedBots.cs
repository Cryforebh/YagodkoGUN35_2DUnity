using System;
using UnityEngine;

public class SphereDetectedBots : MonoBehaviour
{
    private float m_radiusDetectedBots;
    private float m_updateTime = 0.5f;
    private float m_currentUpdateTime;

    public float RariusDetectedBots => m_radiusDetectedBots;

    public event Action<Transform> OtherBotEnterZoneDetectedEvent;
    public event Action<Transform> OtherBotStayZoneDetectedEvent;
    public event Action<Transform> OtherBotExitZoneDetectedEvent;

    private void OnTriggerEnter(Collider other)
    {
        m_currentUpdateTime = m_updateTime;
        OtherBotEnterZoneDetectedEvent?.Invoke(other.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        m_currentUpdateTime -= Time.deltaTime;

        if (m_currentUpdateTime <= 0)
        {
            OtherBotStayZoneDetectedEvent?.Invoke(other.transform);
            m_currentUpdateTime = m_updateTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_currentUpdateTime = m_updateTime;
        OtherBotExitZoneDetectedEvent?.Invoke(other.transform);
    }

    private void Awake()
    {
        m_radiusDetectedBots = transform.localScale.x;
    }
}

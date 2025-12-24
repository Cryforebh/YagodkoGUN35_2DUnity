using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BotBase : MonoBehaviour
{
    private SphereDetectedBots m_detectedBots;
    private Rigidbody m_rigidbody;
    private float m_timeDialogDeley = 20f;
    private bool m_isDialogue = false;
    private List<Transform> m_otherNearBotsContainer = new List<Transform>();

    public bool IsDialogue { get => m_isDialogue; set => m_isDialogue = value; }
    public Rigidbody BotRigidbody { get => m_rigidbody; set => m_rigidbody = value; }
    public SphereDetectedBots GetDetectedBots => m_detectedBots;
    public List<Transform> OtherNearBotsContainer => m_otherNearBotsContainer;


    protected BotStateBase CurrentState;
    public BotStateIlde StateIdle = new();
    public BotStatePatrol StatePatrol = new();
    public BotStateDialogue StateDialogue = new();

    public abstract void LastAwake();
    public abstract void LastOnEnable();
    public abstract void LastOnDisable();

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_detectedBots = GetComponentInChildren<SphereDetectedBots>();
        LastAwake();
    }

    private void OnEnable()
    {
        m_detectedBots.OtherBotEnterZoneDetectedEvent += OtherBotEnterZoneDetected;
        m_detectedBots.OtherBotExitZoneDetectedEvent += OtherBotExitZoneDetected;
        LastOnEnable();
    }

    private void OtherBotExitZoneDetected(Transform obj)
    {
        m_otherNearBotsContainer.Remove(obj);
    }

    private void OnDisable()
    {
        m_detectedBots.OtherBotEnterZoneDetectedEvent -= OtherBotEnterZoneDetected;
        m_detectedBots.OtherBotExitZoneDetectedEvent -= OtherBotExitZoneDetected;
        LastOnDisable();
    }

    private void OtherBotEnterZoneDetected(Transform botTransform)
    {
        if (transform != botTransform)
        {
            m_otherNearBotsContainer.Add(botTransform);
        }
    }

    public void SwitchState(BotStateBase state)
    {
        CurrentState = state;
        CurrentState.EnterState(this);
    }

    public void TimeDialogDeley()
    {
        if (m_isDialogue == true)
        {
            m_timeDialogDeley -= Time.deltaTime;

            if (m_timeDialogDeley <= 0)
            {
                m_isDialogue = false;
                m_timeDialogDeley = 20f;
            }
        }
    }
}



using UnityEngine;
using UnityEngine.AI;

public class Client : BotBase
{
    private bool m_isClient = true;
    private bool m_isToTransfering = false;

    public BotPickUpState IdlePickUpState = new();

    public bool IsClient { get => m_isClient; set => m_isClient = value; }
    public bool IsToTransfering { get => m_isToTransfering; set => m_isToTransfering = value; }

    private void Awake()
    {
        AIAnimator = GetComponent<Animator>();
        AIAgent = GetComponent<NavMeshAgent>();
        AIAudioSource = GetComponent<AudioSource>();

        IsCanWalk = false;
    }

    private void Start()
    {
        AIBotState = IdleState;
        AIBotState.EnterState(this);
    }

    private void Update()
    {
        AIBotState.UpdateState(this);
    }
}

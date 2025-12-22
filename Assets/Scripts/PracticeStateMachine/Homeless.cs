using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Homeless : MonoBehaviour
{
    [SerializeField] private VoiceContainer m_voiceContainer;

    [Inject] private PathWalkContainer m_pathContainer;

    private HomelessStateBase m_currentState;
    public readonly HomelessStateIdle HomelessStateIdleWork = new();
    public readonly HomelessStateSearch HomelessStateSearchWork = new();
    public readonly HomelessStateCollect HomelessStateCollectWork = new();

    private Animator m_characterAnimator;
    private NavMeshAgent m_navMeshAgent;
    private AudioSource m_audioSource;

    public Coin TargetCoin { get; set; }
    public Animator CharacterAnimator => m_characterAnimator;
    public NavMeshAgent NavMeshAgent => m_navMeshAgent;
    public PathWalkContainer PathWalkContainer => m_pathContainer;
    public AudioSource AIAudioSource => m_audioSource;
    public VoiceContainer Voice => m_voiceContainer;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_characterAnimator = GetComponent<Animator>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        m_currentState = HomelessStateIdleWork;

        m_currentState.EnterState(this);
    }

    private void Update()
    {
        m_currentState.UpdateState(this);
    }

    public void SwitchState(HomelessStateBase state)
    {
        m_currentState = state;
        m_currentState.EnterState(this);
    }
}

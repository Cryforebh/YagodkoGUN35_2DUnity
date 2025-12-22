using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent), typeof(AudioSource),typeof(Animator))]
public abstract class BotBase : MonoBehaviour
{
    private NavMeshAgent m_navMeshAgent;
    private AudioSource m_audioSource;
    private Animator m_characterAnimator;
    private BotStateBase m_currentState;
    private PathWalkContainer m_pathContainer;
    public BotIdleState IdleState = new();
    public BotWalkState WalkState = new();
    private bool m_isCanWalk = true;

    public NavMeshAgent AIAgent { get => m_navMeshAgent; set => m_navMeshAgent = value; }
    public AudioSource AIAudioSource { get => m_audioSource; set => m_audioSource = value; }
    public Animator AIAnimator { get => m_characterAnimator; set => m_characterAnimator = value; }
    public PathWalkContainer AllPathWalkContainer { get => m_pathContainer; set => m_pathContainer = value; }
    public BotStateBase AIBotState { get => m_currentState; set => m_currentState = value; }
    public bool IsCanWalk { get => m_isCanWalk; set => m_isCanWalk = value; }

    public void SwitchState(BotStateBase state)
    {
        m_currentState = state;
        m_currentState.EnterState(this);
    }

    [Inject]
    public void AssignPathWalkContainer(PathWalkContainer pathWalkContainer)
    {
        m_pathContainer = pathWalkContainer;
    }
}

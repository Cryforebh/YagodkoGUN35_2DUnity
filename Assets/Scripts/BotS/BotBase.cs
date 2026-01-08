using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent), typeof(AudioSource), typeof(Animator))]
public abstract class BotBase : MonoBehaviour
{
    private PathWalkContainer m_pathContainer;
    [HideInInspector] public NavMeshAgent AIAgent;
    [HideInInspector] public AudioSource AIAudioSource;
    [HideInInspector] public Animator AIAnimator;
    [HideInInspector] public StateOfDanger AIStateOfDanger;
    [HideInInspector] public BotStateBase AICurrentState;
    public Dictionary<IDState, BotStateBase> AllState = new Dictionary<IDState, BotStateBase>();

    public PathWalkContainer AllPathWalkContainer => m_pathContainer;

    public void RegisterState(BotStateBase state)
    {
        AllState.Add(state.GetIDState(), state);
    }

    public void SwitchState(IDState nextState)
    {
        if (AllState.ContainsKey(nextState))
        {
            BotStateBase state = AllState[nextState];
            if (AICurrentState != state)
            {
                AICurrentState?.ExitState(this);
                AICurrentState = state;
                AICurrentState?.EnterState(this);
            }
        }
        else
            Debug.LogError("Не зарегестрированное Состояние!");
    }

    [Inject]
    public void AssignPathWalkContainer(PathWalkContainer pathWalkContainer)
    {
        m_pathContainer = pathWalkContainer;
    }

    public enum StateOfDanger
    {
        Peaceful,
        Hostile
    }

    public void SetStateOfDanger(StateOfDanger stateOfDanger)
    {
        AIStateOfDanger = stateOfDanger;
    }
}

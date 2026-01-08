using UnityEngine;
using UnityEngine.AI;

public class Passerby : BotBase
{
    [SerializeField] private IDState m_startState;

    private void Start()
    {
        AIAnimator = GetComponent<Animator>();
        AIAgent = GetComponent<NavMeshAgent>();
        AIAudioSource = GetComponent<AudioSource>();

        AIStateOfDanger = StateOfDanger.Peaceful;
        AIAgent.speed = 1.4f; /*Random.Range(1.1f, 1.7f);*/

        RegisterState(new BotIdleState());
        RegisterState(new BotWalkState());
        SwitchState(m_startState);
    }

    private void Update()
    {
        AICurrentState?.UpdateState(this);
    }
}

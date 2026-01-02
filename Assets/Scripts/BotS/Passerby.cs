using UnityEngine;
using UnityEngine.AI;

public class Passerby : BotBase
{
    private void Awake()
    {
        AIAnimator = GetComponent<Animator>();
        AIAgent = GetComponent<NavMeshAgent>();
        AIAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        AIStateOfDanger = StateOfDanger.Peaceful;
        AIAgent.speed = Random.Range(2f, 2.3f);
        AIBotState = WalkState;
        AIBotState.EnterState(this);
    }

    private void Update()
    {
        AIBotState.UpdateState(this);
    }
}

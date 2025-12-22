using UnityEngine;
using UnityEngine.AI;

public class Passerby : BotBase
{
    private bool m_isMovingToDestination = false;

    private void Awake()
    {
        AIAnimator = GetComponent<Animator>();
        AIAgent = GetComponent<NavMeshAgent>();
        AIAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        AIAgent.speed = Random.Range(2f, 2.3f);
    }

    public void UpdateDestination()
    {
        if (AllPathWalkContainer == null)
        {
            print("Контейнер не инициализирован!");
            AllPathWalkContainer = FindObjectOfType<PathWalkContainer>();
        }
        AIAgent.destination = AllPathWalkContainer.GetScanneNextPatch(transform);
        m_isMovingToDestination = true;
    }

    private void Update()
    {
        AIAnimator.SetFloat("Movement", AIAgent.velocity.magnitude);

        if (m_isMovingToDestination)
        {
            var distance = Vector3.Distance(transform.position, AIAgent.destination);
            if (distance <= AIAgent.stoppingDistance + 1f)
            {
                m_isMovingToDestination = false;
                UpdateDestination();
            }
        }
    }
}

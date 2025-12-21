using UnityEngine;
using UnityEngine.AI;

public class BotWalking : MonoBehaviour
{
    private PatchWalkContainer m_patchContainer;

    private NavMeshAgent m_meshAgent;
    private Animator m_animator;
    private bool m_isInPatch = false;

    public void SetPatchWalkContainer(PatchWalkContainer patchWalkContainer)
    {
        m_patchContainer = patchWalkContainer;
    }

    private void Awake()
    {
        m_meshAgent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_meshAgent.speed = Random.Range(2f, 2.3f);
    }

    private void OnEnable()
    {
        //SetPatch();
    }

    public void SetPatch()
    {
        if (m_patchContainer == null)
        {
            print("Контейнер не инициализирован!");
            m_patchContainer = FindObjectOfType<PatchWalkContainer>();
        }
        m_meshAgent.destination = m_patchContainer.GetScanneNextPatch(transform);
        m_isInPatch = true;
    }

    private void Update()
    {
        m_animator.SetFloat("Movement", m_meshAgent.velocity.magnitude);

        if (m_isInPatch)
        {
            var distance = Vector3.Distance(transform.position, m_meshAgent.destination);
            if (distance <= m_meshAgent.stoppingDistance + 1f)
            {
                m_isInPatch = false;
                SetPatch();
            }
        }
    }
}

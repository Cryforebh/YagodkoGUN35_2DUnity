using UnityEngine;
using Zenject;

public class PointSpawnBotWalking : MonoBehaviour
{
    [Inject] private SpawnerControlBotWalking m_spawner;

    private Collider m_collider;
    private Bounds m_bounds;
    private float m_timeSpawn = 2f;
    private float m_currentTimeSpawn = 0;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_bounds = m_collider.bounds;
    }

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        m_currentTimeSpawn += Time.deltaTime;

        if (m_currentTimeSpawn >= m_timeSpawn)
        {
            m_currentTimeSpawn = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(m_bounds.min.x + 0.3f, m_bounds.max.x - 0.3f),
            0,
            Random.Range(m_bounds.min.z + 0.3f, m_bounds.max.z - 0.3f)
        );

        m_spawner.CreateBot(randomPoint);
    }
}

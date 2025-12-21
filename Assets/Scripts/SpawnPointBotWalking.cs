using UnityEngine;
using Zenject;

public class SpawnPointBotWalking : MonoBehaviour
{
    [Inject] private SpawnerControlBotWalking m_spawner;
    
    private Collider m_collider;
    private Bounds m_bounds;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_bounds = m_collider.bounds;
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(m_bounds.min.x / 2, m_bounds.max.x / 2),
            0,
            Random.Range(m_bounds.min.z / 2, m_bounds.max.z / 2)
        );
        m_spawner.AddBotInCollection(randomPoint);
    }
}

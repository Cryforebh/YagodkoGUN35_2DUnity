using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnerControlBotWalking : MonoBehaviour
{
    [Inject] private PathWalkContainer m_patchWalkContainer;

    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private float m_minDistanceToSpawnBot = 15f;
    [SerializeField] private float m_minDistanceToDespawnBot = 30f;
    [SerializeField] private int m_maxCountBots = 20;
    [SerializeField] private Passerby m_prefabBotWalking;
    private List<Passerby> m_botWalkings = new List<Passerby>();
    private List<Passerby> m_botsToRemove = new List<Passerby>();

    public bool IsBotFar(Vector3 botPosition)
    {
        return Vector3.Distance(m_playerTransform.position, botPosition) >= m_minDistanceToDespawnBot;
    }

    private void Update()
    {
        RemoveBots();
    }

    public void CreateBot(Vector3 pointSpawnPosition)
    {
        if (m_botWalkings.Count <= m_maxCountBots)
        {
            var distance = Vector3.Distance(m_playerTransform.position, pointSpawnPosition);

            if (distance > m_minDistanceToSpawnBot && distance < m_minDistanceToDespawnBot)
            {
                Passerby bot = Instantiate(m_prefabBotWalking, pointSpawnPosition, Quaternion.identity);
                bot.AssignPathWalkContainer(m_patchWalkContainer);
                m_botWalkings.Add(bot);
            }
        }
    }

    public void RemoveBots()
    {
        foreach (var bot in m_botWalkings)
        {
            if (IsBotFar(bot.transform.position))
            {
                m_botsToRemove.Add(bot);
            }
        }

        if (m_botsToRemove.Count > 0)
            foreach (var bot in m_botsToRemove)
            {
                Destroy(bot.gameObject);
                m_botWalkings.Remove(bot);
            }

        m_botsToRemove.Clear();
    }
}

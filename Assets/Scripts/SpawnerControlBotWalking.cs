using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnerControlBotWalking : MonoBehaviour
{
    [Inject] private PathWalkContainer m_patchWalkContainer;

    [SerializeField] private int m_maxCountBots = 20;
    [SerializeField] private Passerby m_prefabBotWalking;
    private List<Passerby> m_botWalkings = new List<Passerby>();

    public void AddBotInCollection(Vector3 targetTransform)
    {
        if (m_botWalkings.Count <= m_maxCountBots)
        {
            Passerby bot = Instantiate(m_prefabBotWalking, targetTransform, Quaternion.identity);
            bot.AssignPathWalkContainer(m_patchWalkContainer);
            bot.UpdateDestination();
            m_botWalkings.Add(bot);
        }
    }

    public void RemoveBotInCollection(Passerby botWalking)
    {
        m_botWalkings.Remove(botWalking);
    }
}

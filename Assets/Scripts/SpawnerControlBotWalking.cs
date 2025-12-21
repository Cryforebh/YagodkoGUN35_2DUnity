using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnerControlBotWalking : MonoBehaviour
{
    [Inject] private PatchWalkContainer m_patchWalkContainer;

    [SerializeField] private int m_maxCountBots = 20;
    [SerializeField] private BotWalking m_prefabBotWalking;
    private List<BotWalking> m_botWalkings = new List<BotWalking>();

    public void AddBotInCollection(Vector3 targetTransform)
    {
        if (m_botWalkings.Count <= m_maxCountBots)
        {
            BotWalking bot = Instantiate(m_prefabBotWalking, targetTransform, Quaternion.identity);
            bot.SetPatchWalkContainer(m_patchWalkContainer);
            bot.SetPatch();
            m_botWalkings.Add(bot);
        }
    }

    public void RemoveBotInCollection(BotWalking botWalking)
    {
        m_botWalkings.Remove(botWalking);
    }
}
